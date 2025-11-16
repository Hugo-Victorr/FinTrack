import React, { useState } from "react";
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
  List,
} from "antd";
import { Column } from "@ant-design/plots";
import CountUp from "react-countup";

import {
  PeriodOption,
  CurrencyOption,
  KpiCard,
  WeeklyFinancePoint,
  WalletItem,
  TransactionItem,
} from "./types";

const { Title, Text } = Typography;

// -----------------------
// Data sources
// -----------------------

const periodOptions: PeriodOption[] = [
  { label: "Mês atual", value: "current" },
  { label: "Últimos 3 meses", value: "3m" },
  { label: "Últimos 6 meses", value: "6m" },
];

const currencyOptions: CurrencyOption[] = [
  { label: "R$", value: "BRL" },
  { label: "U$D", value: "USD" },
];

const kpis: KpiCard[] = [
  {
    title: "Saldo consolidado",
    value: 7450,
    prefix: "R$ ",
    subtitle: "+4.2% vs mês anterior",
    decimals: 2,
  },
  {
    title: "Total de receitas",
    value: 5450,
    prefix: "R$ ",
    decimals: 2,
    tag: { color: "green", label: "Salário (76%)" },
  },
  {
    title: "Total de despesas",
    value: 3500,
    prefix: "R$ ",
    decimals: 2,
    subtitle: "-3.1% vs mês anterior",
  },
  {
    title: "Desvio de Entrada x Saída",
    value: 23,
    suffix: "%",
    subtitle: "Saldo do mês: R$ 1.300,00",
    decimals: 0,
  },
];

const weeklyData: WeeklyFinancePoint[] = [
  { week: "Semana 1", type: "Receitas", value: 5000 },
  { week: "Semana 1", type: "Despesas", value: 1200 },
  { week: "Semana 2", type: "Despesas", value: 1600 },
  { week: "Semana 3", type: "Despesas", value: 900 },
  { week: "Semana 4", type: "Despesas", value: 800 },
];

const wallets: WalletItem[] = [
  { name: "Nubank", balance: 1200, type: "Conta corrente" },
  { name: "Itaú", balance: 5000, type: "Conta corrente" },
  { name: "Itaú", balance: 5000, type: "Conta corrente" },
  { name: "Itaú", balance: 5000, type: "Conta corrente" },
  { name: "Itaú", balance: 5000, type: "Conta corrente" },
];

const transactions: TransactionItem[] = [
  {
    date: "2025-11-10",
    category: "Alimentação",
    description: "Restaurante",
    value: -78.5,
    wallet: "Nubank",
  },
  {
    date: "2025-11-09",
    category: "Salário",
    description: "Empresa X",
    value: 5400,
    wallet: "Itaú",
  },
];

// -----------------------
// Helpers
// -----------------------

function getCurrencyLabel(value: string, fallback = "") {
  return currencyOptions.find((o) => o.value === value)?.label ?? fallback;
}

// -----------------------
// Component
// -----------------------

export default function FinanceDashboard() {
  const { token } = theme.useToken();
  const [currency, setCurrency] = useState("R$");

  const columns = [
    { title: "Data", dataIndex: "date" },
    {
      title: "Categoria",
      dataIndex: "category",
      render: (cat: string) => <Tag color="blue">{cat}</Tag>,
    },
    { title: "Descrição", dataIndex: "description" },
    {
      title: "Valor",
      dataIndex: "value",
      render: (val: number) => (
        <Text style={{ color: val < 0 ? "red" : "green" }}>
          {val.toLocaleString("pt-BR", { style: "currency", currency: "BRL" })}
        </Text>
      ),
    },
    { title: "Carteira", dataIndex: "wallet" },
  ];

  return (
    <List>
      <Space direction="vertical" size="middle" style={{ display: "flex" }}>
        {/* Header */}
        <Row gutter={[16, 16]} justify="space-between" align="middle">
          <Col>
            <Title level={3}>Dashboard</Title>
          </Col>
          <Col>
            <Select
              defaultValue="current"
              options={periodOptions}
              style={{ width: 160, marginRight: 12 }}
            />
            <Select
              defaultValue="BRL"
              options={currencyOptions}
              style={{ width: 160, marginRight: 12 }}
              onChange={(val) => setCurrency(getCurrencyLabel(val))}
            />
          </Col>
        </Row>

        {/* KPI Cards */}
        <Row gutter={[16, 16]}>
          {kpis.map((kpi, index) => (
            <Col span={6} key={index}>
              <Card>
                <Text>{kpi.title}</Text>
                <Title level={2}>
                  <CountUp
                    end={kpi.value}
                    decimals={kpi.decimals ?? 2}
                    decimal=","
                    separator="."
                    prefix={kpi.prefix}
                    suffix={kpi.suffix}
                  />
                </Title>

                {kpi.tag && <Tag color={kpi.tag.color}>{kpi.tag.label}</Tag>}
                {kpi.subtitle && <Text type="secondary">{kpi.subtitle}</Text>}
              </Card>
            </Col>
          ))}
        </Row>

        {/* Chart */}
        <Card title="Receitas e despesas por semana">
          <Column
            data={weeklyData}
            height={300}
            xField="week"
            yField="value"
            seriesField="type"
            color={(item) => (item.type === "Receitas" ? "green" : "red")}
            colorField={"type"}
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
              text: (val) => `${currency} ${val.value}`,
              style: {
                fill: token.colorText,
                fontSize: 15,
              },
            }}
            viewStyle={{
              viewFill: token.colorBgContainer,
              contentFill: token.colorBgContainer,
            }}
          />
        </Card>

        {/* Wallets */}
        <Card title="Carteiras">
          <Row gutter={[16, 16]}>
            {wallets.map((w, index) => (
              <Col span={8} key={index}>
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
        <Card title="Transações recentes">
          <Table
            dataSource={transactions}
            columns={columns}
            pagination={false}
            rowKey={(r) => r.date + r.description}
          />
        </Card>
      </Space>
    </List>
  );
}
