import React, { useState, useEffect } from 'react';
import { Table, Button, Modal, Form, Input, Spin, Alert, Popconfirm, ConfigProvider, theme, Space } from 'antd';
import { 
  fetchDifficulties, 
  addDifficulty, 
  updateDifficulty, 
  deleteDifficulty 
} from '../../services/api';
import classes from './AdminPanel.module.css';

export const DifficultyAdmin = () => {
  const [difficulties, setDifficulties] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [modalVisible, setModalVisible] = useState(false);
  const [editingDifficulty, setEditingDifficulty] = useState(null);
  const [form] = Form.useForm();

  useEffect(() => {
    loadDifficulties();
  }, [])

  const loadDifficulties = async () => {
    setLoading(true);
    try {
      const data = await fetchDifficulties();
      setDifficulties(data);
    } catch (err) {
      setError(err.message)
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();

      if (editingDifficulty) {
        await updateDifficulty(editingDifficulty.id, values);
      } else {
        await addDifficulty(values);
      }

      setModalVisible(false);
      form.resetFields();
      setEditingDifficulty(null);
      loadDifficulties();
    } catch (err) {
      setError(err.message)
    }
  };

  const handleEdit = (difficulty) => {
    setEditingDifficulty(difficulty);
    form.setFieldsValue({
      difficultyName: difficulty.difficultyName
    })
    setModalVisible(true);
  }

  const handleDelete = async (id) => {
    try {
      await deleteDifficulty(id);
      loadDifficulties();
    } catch (err) {
      setError(err.message)
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
      dataIndex: 'difficultyName',
      key: 'difficultyName'
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
              title='Delete this difficulty?'
              onConfirm={() => handleDelete(record.id)}
              okText='Yes'
              cancelText='No'
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
        Add Difficulty
      </Button>

      {error && <Alert message={error} type="error" showIcon style={{marginBottom: 16}} />}

      <Spin spinning={loading}>
        <Table
          dataSource={difficulties}
          columns={columns}
          rowKey="id"
          pagination={false}
          className={classes.compactTable}
          bordered
        />
      </Spin>

      <Modal
        title={editingDifficulty ? "Edit Difficulty" : "Add Difficulty"}
        open={modalVisible}
        onOk={handleSubmit}
        onCancel={() => {
          setModalVisible(false);
          form.resetFields();
          setEditingDifficulty(null);
        }}
        destroyOnHidden
      >
        <Form form={form} layout="vertical">
          <Form.Item
          name="difficultyName"
          label="Name"
          rules={[{required: true, message: 'Please enter difficulty name'}]}
          >
            <Input />
          </Form.Item>
        </Form>
      </Modal>
    </ConfigProvider>
  );
}