import { useState } from "react";
import {
  Card,
  Row,
  Col,
  Typography,
  Select,
  Table,
  Tag,
  Space,
  theme,
  Spin,
  Tooltip,
} from "antd";
import { Column } from "@ant-design/plots";
import CountUp from "react-countup";

import {
  PeriodOption,
  CurrencyOption,
  WeeklyFinancePoint,
  DashboardResponse,
  TransactionItem,
} from "./types";
import { useCustom } from "@refinedev/core";
import { List } from "@refinedev/antd";
import {
  ArrowDownOutlined,
  ArrowUpOutlined,
  InfoCircleOutlined,
} from "@ant-design/icons";

const { Title, Text } = Typography;

// -----------------------
// Data sources
// -----------------------

const periodOptions: PeriodOption[] = [
  { label: "Mês atual", value: "current" },
  { label: "Últimos 3 meses", value: "3m" },
  { label: "Últimos 6 meses", value: "6m" },
];

// const currencyOptions: CurrencyOption[] = [
//   { label: "R$", value: "BRL" },
//   { label: "U$D", value: "USD" },
// ];

// -----------------------
// Helpers
// -----------------------

// function getCurrencyLabel(value: string, fallback = "") {
//   return currencyOptions.find((o) => o.value === value)?.label ?? fallback;
// }

// -----------------------
// Component
// -----------------------

export default function FinanceDashboard() {
  const { token } = theme.useToken();

  const {
    query: { data: dashResponse, isLoading },
  } = useCustom<DashboardResponse>({
    method: "get",
    url: `http://localhost:5000/api/dashboard`, // TODO: change to the correct url
  });

  const transactions = dashResponse?.data?.recentTransactions;
  const wallets = dashResponse?.data?.wallets;
  const weeklyData = dashResponse?.data?.weeklyData;
  const consolidatedBalance = dashResponse?.data?.consolidatedBalance;
  const totalIncome = dashResponse?.data?.totalIncome;
  const totalExpenses = dashResponse?.data?.totalExpenses;
  const incomeExpenseDeviation = dashResponse?.data?.incomeExpenseDeviation;

  return (
    <List>
      <Spin spinning={isLoading}>
        <Space direction="vertical" size="middle" style={{ display: "flex" }}>
          {/* Header */}
          <Row gutter={[16, 16]} justify="space-between" align="middle">
            <Col>
              <Select
                defaultValue="current"
                options={periodOptions}
                style={{ width: 160, marginRight: 12 }}
              />
              {/* <Select
                defaultValue="BRL"
                options={currencyOptions}
                style={{ width: 160, marginRight: 12 }}
                onChange={(val) => setCurrency(getCurrencyLabel(val))}
              /> */}
            </Col>
          </Row>

          {/* KPI Cards */}
          <Row gutter={[16, 16]}>
            {/* Consolidated Balance */}
            {consolidatedBalance && (
              <Col xs={24} sm={12} md={12} lg={6}>
                <Card
                  style={{ height: "100%" }}
                  title="Saldo consolidado"
                  extra={
                    <Tooltip title="O saldo consolidado é o saldo total de todas as carteiras. Ele é calculado somando o saldo de todas as carteiras e subtraindo o saldo de todas as despesas.">
                      <InfoCircleOutlined style={{ color: token.colorText }} />
                    </Tooltip>
                  }
                >
                  <Title level={2}>
                    <CountUp
                      end={consolidatedBalance.value}
                      decimals={2}
                      decimal=","
                      separator="."
                      prefix="R$ "
                    />
                  </Title>
                  {consolidatedBalance.balanceChange > 0 && (
                    <ArrowUpOutlined style={{ color: "green" }} />
                  )}
                  {consolidatedBalance.balanceChange < 0 && (
                    <ArrowDownOutlined style={{ color: "red" }} />
                  )}
                  <Text type="secondary">
                    {consolidatedBalance.balanceChange >= 0
                      ? ` +${consolidatedBalance.balanceChange.toFixed(
                          1
                        )}% vs mês anterior`
                      : ` ${consolidatedBalance.balanceChange.toFixed(
                          1
                        )}% vs mês anterior`}
                  </Text>
                </Card>
              </Col>
            )}

            {/* Total Income */}
            {totalIncome && (
              <Col xs={24} sm={12} md={12} lg={6}>
                <Card
                  style={{ height: "100%" }}
                  title="Total de receitas"
                  extra={
                    <Tooltip title="O total de receitas é a soma de todas as receitas. Ele é calculado somando o saldo de todas as carteiras.">
                      <InfoCircleOutlined style={{ color: token.colorText }} />
                    </Tooltip>
                  }
                >
                  <Title level={2} style={{ marginBottom: 8 }}>
                    <CountUp
                      end={totalIncome.value}
                      decimals={2}
                      decimal=","
                      separator="."
                      prefix="R$ "
                    />
                  </Title>
                  {totalIncome.topIncomes && totalIncome.topIncomes.length > 0 && (
                    <div style={{ overflowX: "auto", overflowY: "auto", maxHeight: "60px" }}>
                      <Space wrap>
                        {totalIncome.topIncomes.map((income) => (
                          <Tag color={income.categoryColor} key={income.category}>
                            {income.category} ({income.percentage.toFixed(0)}%)
                          </Tag>
                        ))}
                      </Space>
                    </div>
                  )}
                </Card>
              </Col>
            )}

            {/* Total Expenses */}
            {totalExpenses && (
              <Col xs={24} sm={12} md={12} lg={6}>
                <Card
                  style={{ height: "100%" }}
                  title="Total de despesas"
                  extra={
                    <Tooltip title="O total de despesas é a soma de todas as despesas. Ele é calculado somando o saldo de todas as carteiras.">
                      <InfoCircleOutlined style={{ color: token.colorText }} />
                    </Tooltip>
                  }
                >
                  <Title level={2}>
                    <CountUp
                      end={totalExpenses.value}
                      decimals={2}
                      decimal=","
                      separator="."
                      prefix="R$ "
                    />
                  </Title>
                  {totalExpenses.expenseChange > 0 && (
                    <ArrowUpOutlined style={{ color: "red" }} />
                  )}
                  {totalExpenses.expenseChange < 0 && (
                    <ArrowDownOutlined style={{ color: "green" }} />
                  )}
                  <Text type="secondary">
                    {totalExpenses.expenseChange >= 0
                      ? ` +${totalExpenses.expenseChange.toFixed(
                          1
                        )}% vs mês anterior`
                      : ` ${totalExpenses.expenseChange.toFixed(
                          1
                        )}% vs mês anterior`}
                  </Text>
                </Card>
              </Col>
            )}

            {/* Income Expense Deviation */}
            {incomeExpenseDeviation && (
              <Col xs={24} sm={12} md={12} lg={6}>
                <Card
                  style={{ height: "100%" }}
                  title="Desvio de Entrada x Saída"
                  extra={
                    <Tooltip title="O desvio de entrada x saída é a diferença entre as receitas e as despesas. Ele é calculado subtraindo o total de despesas do total de receitas.">
                      <InfoCircleOutlined style={{ color: token.colorText }} />
                    </Tooltip>
                  }
                >
                  <Title level={2}>
                    <CountUp
                      end={incomeExpenseDeviation.value}
                      decimals={0}
                      decimal=","
                      separator="."
                      suffix=" %"
                    />
                  </Title>
                  <Text type="secondary">
                    Saldo do mês:
                    {
                      <span
                        style={{
                          color:
                            incomeExpenseDeviation.monthlyBalance > 0
                              ? "green"
                              : "red",
                        }}
                      >
                        {" "}
                        R$ {incomeExpenseDeviation.monthlyBalance.toFixed(2)}
                      </span>
                    }
                  </Text>
                </Card>
              </Col>
            )}
          </Row>

          {/* Chart */}
          <Card
            title="Receitas e despesas por semana"
            extra={
              <Tooltip title="O gráfico de receitas e despesas por semana é um gráfico que mostra as receitas e despesas por semana. Ele é calculado somando as receitas e despesas por semana.">
                <InfoCircleOutlined style={{ color: token.colorText }} />
              </Tooltip>
            }
          >
            <Column
              data={weeklyData ?? []}
              height={300}
              xField="week"
              yField="value"
              seriesField="type"
              colorField="type"
              scale={{
                color: {
                  palette: [token.colorSuccess, token.colorError],
                  domain: ["Receitas", "Despesas"],
                },
              }}
              legend={{
                color: {
                  itemLabelFill: token.colorText,
                  position: "bottom",
                },
              }}
              axis={{
                y: {
                  labelFill: token.colorText,
                  grid: true,
                  gridStroke: token.colorText,
                  gridStrokeOpacity: 0.5,
                },
                x: {
                  labelFill: token.colorText,
                },
              }}
              label={{
                textBaseline: "bottom",
                text: (val: WeeklyFinancePoint) => `R$ ${val.value}`,
                style: {
                  fill: token.colorText,
                  fontSize: 15,
                },
              }}
              viewStyle={{
                viewFill: token.colorBgContainer,
                contentFill: token.colorBgContainer,
              }}
              style={{
                maxWidth: 150,
                radiusTopLeft: 10,
                radiusTopRight: 10,
              }}
            />
          </Card>

          {/* Wallets */}
          <Card
            title="Carteiras"
            extra={
              <Tooltip title="As carteiras são as carteiras que você tem. Elas são usadas para categorizar as transações.">
                <InfoCircleOutlined style={{ color: token.colorText }} />
              </Tooltip>
            }
          >
            <Row gutter={[16, 16]}>
              {wallets?.map((w, index) => (
                <Col xs={24} sm={12} md={8} lg={8} key={index}>
                  <Card size="small">
                    <Text strong>{w.name}</Text>
                    <Title level={4}>
                      {w.balance.toLocaleString("pt-BR", {
                        style: "currency",
                        currency: "BRL",
                      })}
                    </Title>
                    <Text type="secondary">{w.type}</Text>
                  </Card>
                </Col>
              ))}
            </Row>
          </Card>

          {/* Transactions */}
          <Card
            title="Transações recentes"
            extra={
              <Tooltip title="As transações recentes são as transações que você tem. Elas são usadas para mostrar as transações mais recentes.">
                <InfoCircleOutlined style={{ color: token.colorText }} />
              </Tooltip>
            }
          >
            <Table
              dataSource={transactions}
              columns={[
                { title: "Data", dataIndex: "date" },
                {
                  title: "Categoria",
                  dataIndex: "category",
                  render: (cat: string, record: TransactionItem) => <Tag color={record.categoryColor}>{cat}</Tag>,
                },
                { title: "Descrição", dataIndex: "description" },
                {
                  title: "Valor",
                  dataIndex: "value",
                  render: (val: number) => (
                    <Tag color={val < 0 ? "red" : "green"}>
                      {val.toLocaleString("pt-BR", {
                        style: "currency",
                        currency: "BRL",
                      })}
                    </Tag>
                  ),
                },
                { title: "Carteira", dataIndex: "wallet" },
              ]}
              pagination={false}
              rowKey={(r) => r.date + r.description}
            />
          </Card>
        </Space>
      </Spin>
    </List>
  );
}
