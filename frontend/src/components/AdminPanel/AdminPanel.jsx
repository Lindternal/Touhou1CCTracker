import React, { useState } from 'react';
import { Tabs } from 'antd';
import { DifficultyAdmin } from './DifficultyAdmin.jsx'
import { SettingsAdmin } from './SettingsAdmin.jsx';
import { GameAdmin } from './GameAdmin.jsx';
import { ShotTypeAdmin } from './ShotTypeAdmin.jsx';
import { RecordAdmin } from './RecordAdmin.jsx';
import classes from './AdminPanel.module.css';

export const AdminPanel = () => {
  const [activeTab, setActiveTab] = useState('difficulties');
  
  const items = [
    {
      key: 'difficulties',
      label: 'Difficulties',
      children: <DifficultyAdmin />
    },
    {
      key: 'games',
      label: 'Games',
      children: <GameAdmin />
    },
    {
      key: 'shotTypes',
      label: 'Shot Types',
      children: <ShotTypeAdmin />
    },
    {
      key: 'records',
      label: 'Records',
      children: <RecordAdmin />
    },
    {
      key: 'settings',
      label: 'Settings',
      children: <SettingsAdmin />
    }
  ];

  return (
    <div className={classes.adminContainer}>
      <div className={classes.tabsContainer}>
        <Tabs
          activeKey={activeTab}
          onChange={setActiveTab}
          items={items}
          tabPosition="left"
          size="large"
        />
      </div>
    </div>
   
  );
};