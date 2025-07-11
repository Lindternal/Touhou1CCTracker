import React from "react";
import { Layout, Space, Typography, Divider } from 'antd';
import { YoutubeOutlined, TwitchOutlined, GithubOutlined } from '@ant-design/icons';
import classes from './Footer.module.css'

const { Footer: AntFooter } = Layout;
const { Link, Text } = Typography;

export const Footer = () => {
  return (
    <AntFooter className={classes.siteFooter}>
    <Divider className={classes.divider}/>
    <Space direction="vertical" size="middle" style={{ width: '100%' }}>
      <div className={classes.footerLinks}>
        <Space size="large">
          <Link href="https://www.youtube.com/@lindternal" target="_blank" rel="noopener noreferrer">
            <YoutubeOutlined /> YouTube
          </Link>
          <Link href="https://www.twitch.tv/lindternal" target="_blank" rel="noopener noreferrer">
            <TwitchOutlined /> Twitch
          </Link>
        </Space>
      </div>
        
      <div>
        <Space direction="vertical" size="small">
          <Text className={classes.footerInfo}>Touhou 1CC Tracker ©{new Date().getFullYear()}</Text>
          <Text className={classes.footerInfo}>Made by Lindternal</Text>
        </Space>
      </div>
    </Space>
    </AntFooter>           
  );
};