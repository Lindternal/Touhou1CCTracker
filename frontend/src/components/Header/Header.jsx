import React, { useState } from 'react';
import classes from './Header.module.css';
import { Button, Space, Dropdown, ConfigProvider, theme } from 'antd';
import { UserOutlined, DownOutlined, LoginOutlined } from '@ant-design/icons';
import { AuthModal } from '../Auth/AuthModal';
import { logout } from '../../services/api';

export function Header({ user, setUser }) {
  const [modalVisible, setModalVisible] = useState(false);

  const handleLogout = async () => {
    await logout();
    setUser(null);
  };

  const items = [
    { key: 'logout', label: 'Log Out', onClick: handleLogout }
  ];

  return(
    <ConfigProvider theme={{algorithm: theme.darkAlgorithm}}>
      <header className={classes.header}>
        <h1 className={classes.headerText}>Touhou 1CC Tracker</h1>
        <div className={classes.authSection}>
          {user ? (
            <Dropdown menu={{items}} trigger={['click']}>
              <Button type='text' style={{color: 'white'}}>
                <Space>
                  <UserOutlined style={{ color: 'white' }} />
                  {user.userName}
                  <DownOutlined style={{ color: 'white' }} />
                </Space>
              </Button>
            </Dropdown>
          ) : (
            <Button
              icon={<LoginOutlined />}
              onClick={() => setModalVisible(true)}
            >
              Sing In
            </Button>
          )}
        </div>
        <AuthModal
          visible={modalVisible}
          onClose={() => setModalVisible(false)}
          onLoginSuccess={setUser}
        />
      </header>
    </ConfigProvider>
  );
}