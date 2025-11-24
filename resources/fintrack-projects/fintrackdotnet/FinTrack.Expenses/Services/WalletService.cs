using AutoMapper;
using FinTrack.Database.EFDao;
using FinTrack.Database.Interfaces;
using FinTrack.Expenses.DTOs;

namespace FinTrack.Expenses.Services;

public class WalletService
{
    private readonly IWalletRepository _repository;
    private readonly IMapper _mapper;

    public WalletService(IWalletRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<WalletDto>> GetAllAsync(QueryOptions opts, Guid userId)
    {
        opts.Filters ??= new Dictionary<string, string>();
        opts.Filters["User"] = userId.ToString();
        
        var entities = await _repository.AllAsync(opts, false);
        return _mapper.Map<List<WalletDto>>(entities);
    }

    public async Task<WalletDto?> GetByIdAsync(Guid id, Guid userId)
    {
        var entity = await _repository.FindAsync(id);
        if (entity is null) return null;
        
        if (entity.User != userId) return null; // Return null for security (don't reveal entity exists)
        
        return _mapper.Map<WalletDto>(entity);
    }

    public async Task<WalletDto> CreateAsync(WalletCreateDto dto, Guid userId)
    {
        var entity = _mapper.Map<FinTrack.Model.Entities.Wallet>(dto);
        entity.User = userId;
        await _repository.AddAsync(entity);
        return _mapper.Map<WalletDto>(entity);
    }

    public async Task<WalletDto?> UpdateAsync(Guid id, WalletUpdateDto dto, Guid userId)
    {
        var entity = await _repository.FindAsync(id, true);
        if (entity is null) return null;
        
        if (entity.User != userId) return null; // Return null for security (don't reveal entity exists)

        _mapper.Map(dto, entity);
        await _repository.UpdateAsync(entity);
        return _mapper.Map<WalletDto>(entity);
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var entity = await _repository.FindAsync(id);
        if (entity is null) return false;
        
        if (entity.User != userId) return false; // Return false for security

        await _repository.DeleteAsync(id);
        return true;
    }

    public async Task<bool> RestoreAsync(Guid id, Guid userId)
    {
        var entity = await _repository.FindAsync(id, true);
        if (entity is null) return false;
        
        if (entity.User != userId) return false; // Return false for security
        
        await _repository.RestoreAsync(entity);
        return true;
    }
}
