import React, { useState, useEffect } from 'react';
import { Table, Button, Modal, Form, Input, Spin, Alert, Popconfirm, ConfigProvider, theme, Space } from 'antd';
import { 
  fetchShotTypes, 
  addShotType, 
  updateShotType, 
  deleteShotType 
} from '../../services/api';
import classes from './AdminPanel.module.css';

export const ShotTypeAdmin = () => {
  const [shotTypes, setShotTypes] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [modalVisible, setModalVisible] = useState(false);
  const [editingShotType, setEditingShotType] = useState(null);
  const [form] = Form.useForm();

  useEffect(() => {
    loadShotTypes();
  }, []);

  const loadShotTypes = async () => {
    setLoading(true);
    try {
      const data = await fetchShotTypes();
      setShotTypes(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();

      if (editingShotType) {
        await updateShotType(editingShotType.id, values);
      } else {
        await addShotType(values);
      }
      
      setModalVisible(false);
      form.resetFields();
      setEditingShotType(null);
      loadShotTypes();
    } catch (err) {
      setError(err.message);
    }
  };

  const handleEdit = (shotType) => {
    setEditingShotType(shotType);
    form.setFieldsValue({
      characterName: shotType.characterName,
      shotName: shotType.shotName
    });
    setModalVisible(true);
  };

  const handleDelete = async (id) => {
    try {
      await deleteShotType(id);
      loadShotTypes();
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
      title: 'Character',
      dataIndex: 'characterName',
      key: 'characterName'
    },
    {
      title: 'Shot Type',
      dataIndex: 'shotName',
      key: 'shotName'
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
            title="Delete this shot type?"
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
        Add Shot Type
      </Button>

      {error && <Alert message={error} type="error" showIcon style={{ marginBottom: 16 }} />}
      
      <Spin spinning={loading}>
        <Table 
          dataSource={shotTypes} 
          columns={columns} 
          rowKey="id"
          pagination={false}
          className={classes.compactTable}
          bordered
        />
      </Spin>

      <Modal
        title={editingShotType ? "Edit Shot Type" : "Add Shot Type"}
        open={modalVisible}
        onOk={handleSubmit}
        onCancel={() => {
          setModalVisible(false);
          form.resetFields();
          setEditingShotType(null);
        }}
        destroyOnHidden
      >
        <Form form={form} layout="vertical">
          <Form.Item
            name="characterName"
            label="Character Name"
            rules={[{ required: true, message: 'Please enter character name' }]}
          >
            <Input />
          </Form.Item>
          <Form.Item
            name="shotName"
            label="Shot Type"
          >
            <Input />
          </Form.Item>
        </Form>
      </Modal>
    </ConfigProvider>
  );
}