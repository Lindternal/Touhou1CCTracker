import React, { useState, useEffect } from 'react';
import { Form, Switch, Spin, Alert, ConfigProvider, theme, message } from 'antd';
import { fetchSettings, updateSetting } from '../../services/api';
import classes from './AdminPanel.module.css';

export const SettingsAdmin = () => {
  const [settings, setSettings] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [registrationEnabled, setRegistrationEnabled] = useState(false);

  useEffect(() => {
    loadSettings();
  }, []);

  const loadSettings = async () => {
    setLoading(true);
    try {
      const data = await fetchSettings();
      setSettings(data);

      const registrationSetting = data.find(s => s.settingName === 'IsRegistrationEnabled');
      if (registrationSetting) {
        setRegistrationEnabled(registrationSetting.settingValue === 'true');
      }
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleSettingChange = async (checked) => {
    try {
      setRegistrationEnabled(checked);
      await updateSetting({
        settingName: 'IsRegistrationEnabled',
        settingValue: checked.toString()
      });
      message.success('Setting updated successfully!');
      loadSettings();
    } catch (err) {
      setRegistrationEnabled(!checked);
      message.error(`Update failed!`);
    }
  };

  return (
    <ConfigProvider theme={{algorithm: theme.darkAlgorithm}}>
      {error && <Alert message={error} type="error" showIcon style={{ marginBottom: 16 }} />}
      
      <Spin spinning={loading}>
        <Form layout="vertical">
          <Form.Item label="User Registration" className={classes.settingSwitch}>
            <Switch
                checked={registrationEnabled}
                onChange={handleSettingChange}
            />
            <div style={{ marginTop: 8 }}>
              <span>
                Registration is 
                <strong>
                  {registrationEnabled ? ' enabled' : ' disabled'}
                </strong>
              </span>
            </div>
          </Form.Item>
        </Form>
      </Spin>
    </ConfigProvider>
  );
};