using AutoMapper;
using FinTrack.Database.EFDao;
using FinTrack.Database.Interfaces;
using FinTrack.Expenses.DTOs;
using FinTrack.Model.Enums;

namespace FinTrack.Expenses.Services;

public class ExpenseService
{
    private readonly IExpenseRepository _repository;
    private readonly IExpenseCategoryRepository _categoryRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IMapper _mapper;

    public ExpenseService(IExpenseRepository repository, IExpenseCategoryRepository categoryRepository, IWalletRepository walletRepository, IMapper mapper)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _walletRepository = walletRepository;
        _mapper = mapper;
    }

    public async Task<List<ExpenseDto>> GetAllAsync(QueryOptions opts, Guid userId)
    {
        opts.Filters ??= new Dictionary<string, string>();
        opts.Filters["User"] = userId.ToString();
        
        var entities = await _repository.AllAsync(
            opts,
            false,
            e => e.ExpenseCategory,
            e => e.Wallet
        );
        return _mapper.Map<List<ExpenseDto>>(entities);
    }

    public async Task<ExpenseDto?> GetByIdAsync(Guid id, Guid userId)
    {
        var entity = await _repository.FindAsync(id, false, e => e.ExpenseCategory, e => e.Wallet);
        if (entity is null) return null;
        
        if (entity.User != userId) return null; // Return null for security (don't reveal entity exists)
        
        return _mapper.Map<ExpenseDto>(entity);
    }

    public async Task<ExpenseDto> CreateAsync(ExpenseCreateDto dto, Guid userId)
    {
        // Get the category to check operation type
        var category = await _categoryRepository.FindAsync(dto.ExpenseCategoryId);
        if (category is null)
        {
            throw new ArgumentException("Expense category not found", nameof(dto.ExpenseCategoryId));
        }

        var entity = _mapper.Map<FinTrack.Model.Entities.Expense>(dto);
        entity.User = userId;
        
        // Make amount negative for expenses, keep positive for incomes
        if (category.OperationType == OperationType.Expense && entity.Amount > 0)
        {
            entity.Amount = -entity.Amount;
        }
        else if (category.OperationType == OperationType.Income && entity.Amount < 0)
        {
            entity.Amount = Math.Abs(entity.Amount);
        }
        
        await _repository.AddAsync(entity);
        
        // Update wallet balance: add the expense amount (negative for expenses, positive for incomes)
        var wallet = await _walletRepository.FindAsync(dto.WalletId, true);
        if (wallet is not null)
        {
            wallet.Amount += entity.Amount;
            await _walletRepository.UpdateAsync(wallet);
        }
        
        return _mapper.Map<ExpenseDto>(entity);
    }

    public async Task<ExpenseDto?> UpdateAsync(Guid id, ExpenseUpdateDto dto, Guid userId)
    {
        var entity = await _repository.FindAsync(id, true);
        if (entity is null) return null;
        
        if (entity.User != userId) return null; // Return null for security (don't reveal entity exists)

        // Get the category to check operation type
        var category = await _categoryRepository.FindAsync(dto.ExpenseCategoryId);
        if (category is null)
        {
            throw new ArgumentException("Expense category not found", nameof(dto.ExpenseCategoryId));
        }

        // Store old values before mapping
        var oldAmount = entity.Amount;
        var oldWalletId = entity.WalletId;

        _mapper.Map(dto, entity);
        
        // Make amount negative for expenses, keep positive for incomes
        if (category.OperationType == OperationType.Expense && entity.Amount > 0)
        {
            entity.Amount = -entity.Amount;
        }
        else if (category.OperationType == OperationType.Income && entity.Amount < 0)
        {
            entity.Amount = Math.Abs(entity.Amount);
        }
        
        await _repository.UpdateAsync(entity);
        
        // Update wallet balances: reverse old amount, apply new amount
        // If wallet changed, update both wallets
        if (oldWalletId != entity.WalletId)
        {
            // Reverse old amount from old wallet
            var oldWallet = await _walletRepository.FindAsync(oldWalletId, true);
            if (oldWallet is not null)
            {
                oldWallet.Amount -= oldAmount; // Reverse the old amount
                await _walletRepository.UpdateAsync(oldWallet);
            }
            
            // Apply new amount to new wallet
            var newWallet = await _walletRepository.FindAsync(entity.WalletId, true);
            if (newWallet is not null)
            {
                newWallet.Amount += entity.Amount;
                await _walletRepository.UpdateAsync(newWallet);
            }
        }
        else
        {
            // Same wallet: apply the difference
            var wallet = await _walletRepository.FindAsync(entity.WalletId, true);
            if (wallet is not null)
            {
                wallet.Amount = wallet.Amount - oldAmount + entity.Amount;
                await _walletRepository.UpdateAsync(wallet);
            }
        }
        
        return _mapper.Map<ExpenseDto>(entity);
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var entity = await _repository.FindAsync(id);
        if (entity is null) return false;
        
        if (entity.User != userId) return false; // Return false for security

        // Store values before deletion
        var amount = entity.Amount;
        var walletId = entity.WalletId;

        await _repository.DeleteAsync(id);
        
        // Reverse the expense amount from wallet (subtract the amount, which reverses the transaction)
        var wallet = await _walletRepository.FindAsync(walletId, true);
        if (wallet is not null)
        {
            wallet.Amount -= amount; // Reverse: if amount was -100 (expense), subtracting -100 adds 100 back
            await _walletRepository.UpdateAsync(wallet);
        }
        
        return true;
    }

    public async Task<bool> RestoreAsync(Guid id, Guid userId)
    {
        var entity = await _repository.FindAsync(id, true);
        if (entity is null) return false;
        
        if (entity.User != userId) return false; // Return false for security
        
        // Store values before restore
        var amount = entity.Amount;
        var walletId = entity.WalletId;
        
        await _repository.RestoreAsync(entity);
        
        // Apply the expense amount back to wallet
        var wallet = await _walletRepository.FindAsync(walletId, true);
        if (wallet is not null)
        {
            wallet.Amount += amount;
            await _walletRepository.UpdateAsync(wallet);
        }
        
        return true;
    }
}
