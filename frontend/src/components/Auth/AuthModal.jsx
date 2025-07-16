import React, { useState } from 'react';
import { Modal, Tabs, Form, Alert } from 'antd';
import { login, register } from '../../services/api';
import { AuthForm } from './AuthForm';

export const AuthModal = ({ visible, onClose, onLoginSuccess }) => {
  const [activeTab, setActiveTab] = useState('login');
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);
  const [form] = Form.useForm();

  const handleSubmit = async (values) => {
    setLoading(true);
    try {
      if (activeTab === 'login') {
        const userData = await login(values);
        onLoginSuccess(userData);
      } else {
        await register(values);
        const userData = await login(values);
        onLoginSuccess(userData);
      }
      handleClose();
    } catch (err) {
        setError(err.message);
    } finally {
        setLoading(false);
    }
  };

  const handleClose = () => {
    form.resetFields();
    setError(null);
    setLoading(false);
    onClose();
  }

  const handleTabChange = (key) => {
    form.resetFields();
    setError(null);
    setLoading(false);
    setActiveTab(key);
  }

  const items = [
    {
      key: 'login',
      label: 'Log In',
      children: <AuthForm form={form} onSubmit={handleSubmit} loading={loading} tabKey='login' />
    },
    {
      key: 'register',
      label: 'Register',
      children: <AuthForm form={form} onSubmit={handleSubmit} loading={loading} withConfirmation tabKey='register' />
    }
  ];

  return (
    <Modal
      title={activeTab === 'login' ? 'Sign In' : 'Sign Up'}
      open={visible}
      onCancel={handleClose}
      footer={null}
      centered
      destroyOnHidden
    >
      <Tabs 
        activeKey={activeTab}
        onChange={handleTabChange}
        items={items}
        destroyOnHidden
      />
      {error && <Alert message={error} type='error' showIcon />}
    </Modal>
  );
}