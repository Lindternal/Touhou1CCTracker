import React, { useState }  from 'react';
import classes from './RecentCardItem.module.css';
import { Card, Typography, Button, Space } from 'antd';
import { DownloadOutlined, PlayCircleOutlined } from '@ant-design/icons';
import { fetchDownloadLink } from '../../services/api.jsx';

const { Text, Title } = Typography;

export const RecentCardItem = ({ record }) => {
  const [downloading, setDownloading] = useState(false);

  const handleDownload = async () => {
    if (!record?.id) return;
  
    setDownloading(true);
  
    try {
      const response = await fetchDownloadLink(record.id);
  
      const blob = await response.blob();
      const contentDisposition = response.headers.get('Content-Disposition');
  
      let filename = 'filename.rpy';
  
      if (contentDisposition) {
        const filenameRegex = /(?:filename="([^"]+)"|filename\*=UTF-8''([^;]+))/i;
        const match = contentDisposition.match(filenameRegex);
        if (match) filename = match[1] || decodeURIComponent(match[2]);
      }
  
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = url;
  
      link.download = filename;
      document.body.appendChild(link);
      link.click();
          
      setTimeout(() => {
        window.URL.revokeObjectURL(link.href);
        document.body.removeChild(link);
      }, 100);
      } catch (err) {
        console.error('Download error:', err);
      } finally {
        setDownloading(false);
    }
  };
  
  const handleWatchVideo = () => {
    window.open(record.videoUrl, '_blank', 'noopener,noreferrer');
  };
  
  const shotType = `${record.characterName} ${record.shotName}`.trim();

  return (
    <Card className={classes.recentCard}>
      <div className={classes.cardHeader}>
        <Title level={4} className={classes.cardTitle}>
          {record.gameName} - {record.rank}
        </Title>
        <Space>
          {record.videoUrl && (
            <Button
              icon={<PlayCircleOutlined />}
              onClick={handleWatchVideo}
              className={classes.actionButton}
            />
          )}
          {record.hasReplayFile && (
            <Button
              icon={<DownloadOutlined />}
              onClick={handleDownload}
              loading={downloading}
              className={classes.downloadButton}
            />
          )}
        </Space>
      </div>
      <div className={classes.cardContent}>
        <Text strong className={classes.text}>Difficulty: </Text>
        <Text className={classes.desc}>{record.difficultyName}</Text>

        <Text strong className={classes.text}>Shot Type: </Text>
        <Text className={classes.desc}>{shotType}</Text>

        <Text strong className={classes.text}>Clear Date: </Text>
        <Text className={classes.desc}>{record.date}</Text>
      </div>
    </Card>
  );
};