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

const { Title, Text } = Typography;

export default function FinanceDashboard() {
  const { token } = theme.useToken();
  const [currency, setCurrency] = useState("R$");

  const periodOptions = [
    { label: "Mês atual", value: "current" },
    { label: "Últimos 3 meses", value: "3m" },
    { label: "Últimos 6 meses", value: "6m" },
  ];

  const currencyOptions = [
    { label: "R$", value: "BRL" },
    { label: "U$D", value: "USD" },
  ];

  function getCurrencyLabel(value: string, fallback = "") {
    return currencyOptions.find((o) => o.value === value)?.label ?? fallback;
  }

  const transactions = [
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

  const columns = [
    { title: "Data", dataIndex: "date" },
    {
      title: "Categoria",
      dataIndex: "category",
      render: (cat) => <Tag color="blue">{cat}</Tag>,
    },
    { title: "Descrição", dataIndex: "description" },
    {
      title: "Valor",
      dataIndex: "value",
      render: (val) => (
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
            onChange={(val) => {
              console.log(val);
              const cur = getCurrencyLabel(val);
              setCurrency(cur);
            }}
          />
        </Col>
      </Row>

      {/* Cards */}
      <Row gutter={[16, 16]}>
        <Col span={6}>
          <Card>
            <Text>Saldo consolidado</Text>
            <Title level={2}>
              <CountUp
                decimals={2}
                decimal=","
                separator="."
                end={7450.0}
                duration={1.5}
                prefix={`${currency} `}
              />
            </Title>
            <Text type="secondary">+4.2% vs mês anterior</Text>
          </Card>
        </Col>
        <Col span={6}>
          <Card>
            <Text>Total de receitas</Text>
            <Title level={2}>
              <CountUp
                decimals={2}
                decimal=","
                separator="."
                end={5450.0}
                duration={2}
                prefix={`${currency} `}
              />
            </Title>
            <Tag color="green">Salário (76%)</Tag>
          </Card>
        </Col>
        <Col span={6}>
          <Card>
            <Text>Total de despesas</Text>
            <Title level={2}>
              <CountUp
                decimals={2}
                decimal=","
                separator="."
                end={3500.0}
                duration={2.5}
                prefix={`${currency} `}
              />
            </Title>
            <Text type="secondary">-3.1% vs mês anterior</Text>
          </Card>
        </Col>
        <Col span={6}>
          <Card>
            <Text>Desvio de Entrada x Saída</Text>
            <Title level={2}>
              <CountUp
                end={23}
                duration={3}
                suffix={`%`}
              />
            </Title>
            <Text type="secondary">Saldo do mês: R$ 1.300,00</Text>
          </Card>
        </Col>
      </Row>

      {/* Grafico */}
      <Card title="Receitas e despesas por semana">
        <Column
          height={300}
          legend={{
            color: {
              itemLabelFill: token.colorText,
              position: "bottom",
              // layout: {
              //   justifyContent: "center", // Main axis (vertical) center
              // },
            },
          }}
          colorField={"type"}
          data={[
            { week: "Semana 1", type: "Receitas", value: 5000, color: "red" },
            { week: "Semana 1", type: "Despesas", value: 1200, color: "green" },
            { week: "Semana 2", type: "Despesas", value: 1600, color: "green" },
            { week: "Semana 3", type: "Despesas", value: 900, color: "green" },
            { week: "Semana 4", type: "Despesas", value: 800, color: "green" },
          ]}
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
          seriesField="type"
          xField="week"
          yField="value"
        />
      </Card>

      {/* Carteiras */}
      <Card title="Carteiras" className="mt-4">
        <Row gutter={[16, 16]}>
          <Col span={8}>
            <Card size="small">
              <Text strong>Nubank</Text>
              <Title level={4}>R$ 1.200,00</Title>
              <Text type="secondary">Conta corrente</Text>
            </Card>
          </Col>
          <Col span={8}>
            <Card size="small">
              <Text strong>Itaú</Text>
              <Title level={4}>R$ 5.000,00</Title>
              <Text type="secondary">Conta corrente</Text>
            </Card>
          </Col>
        </Row>
      </Card>

      {/* Transações recentes */}
      <Card title="Transações recentes">
        <Table dataSource={transactions} columns={columns} pagination={false} />
      </Card>
    </Space>
    </List>
  );
}
