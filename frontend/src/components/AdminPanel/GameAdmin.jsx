import React, { useState, useEffect } from 'react';
import { Table, Button, Modal, Form, Input, Spin, Alert, Popconfirm, ConfigProvider, theme, Space } from 'antd';
import { 
  fetchGames, 
  addGame, 
  updateGame, 
  deleteGame 
} from '../../services/api';
import classes from './AdminPanel.module.css';

export const GameAdmin = () => {
  const [games, setGames] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [modalVisible, setModalVisible] = useState(false);
  const [editingGame, setEditingGame] = useState(null);
  const [form] = Form.useForm();

  useEffect(() => {
    loadGames();
  }, []);

  const loadGames = async () => {
    setLoading(true);
    try {
      const data = await fetchGames();
      setGames(data);
    } catch (err) {
        setError(err.message);
    } finally {
        setLoading(false);
    }
  };

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      
      if (editingGame) {
        await updateGame(editingGame.id, values);
      } else {
        await addGame(values);
      }

      setModalVisible(false);
      form.resetFields();
      setEditingGame(null);
      loadGames();
    } catch (err) {
      setError(err.message)
    }
  };

  const handleEdit = (game) => {
    setEditingGame(game);
    form.setFieldsValue({
        gameName: game.gameName
    });
    setModalVisible(true);
  };

  const handleDelete = async (id) => {
    try {
      await deleteGame(id);
      loadGames();
    } catch (err) {
      setError(err.message);
    }
  };

  const columns = [
    {
      title: 'ID',
      dataIndex: 'id',
      key: 'id',
      width: 80
    },
    {
      title: 'Name',
      dataIndex: 'gameName',
      key: 'gameName'
    },
    {
      title: 'Actions',
      key: 'actions',
      width: 150,
      render: (_, record) => (
        <Space size="small" className={classes.tableActions}>
          <Button type="primary" onClick={() => handleEdit(record)}>
            Edit
          </Button>
          <Popconfirm
            title="Delete this game?"
            onConfirm={() => handleDelete(record.id)}
            okText="Yes"
            cancelText="No"
          >
            <Button type="primary" danger>
              Delete
            </Button>
          </Popconfirm>
        </Space>
      )
    }
  ];

  return (
    <ConfigProvider theme={{algorithm: theme.darkAlgorithm}}>
      <Button type="primary" onClick={() => setModalVisible(true)} className={classes.addButton}>
        Add Game
      </Button>

      {error && <Alert message={error} type="error" showIcon style={{ marginBottom: 16 }} />}
      
      <Spin spinning={loading}>
        <Table 
          dataSource={games} 
          columns={columns} 
          rowKey="id"
          pagination={false}
          className={classes.compactTable}
          bordered
        />
      </Spin>

      <Modal
        title={editingGame ? "Edit Game" : "Add Game"}
        open={modalVisible}
        onOk={handleSubmit}
        onCancel={() => {
          setModalVisible(false);
          form.resetFields();
          setEditingGame(null);
        }}
        destroyOnHidden
      >
        <Form form={form} layout="vertical">
          <Form.Item
            name="gameName"
            label="Name"
            rules={[{ required: true, message: 'Please enter game name' }]}
          >
            <Input />
          </Form.Item>
        </Form>
      </Modal>
    </ConfigProvider>
  );
}