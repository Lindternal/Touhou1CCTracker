import React, { useState } from 'react';
import { Card, Typography, Button, Divider, Space, Spin } from 'antd';
import { DownloadOutlined, PlayCircleOutlined } from '@ant-design/icons';
import classes from '../CardContainer/CardContainer.module.css';
import { fetchDownloadLink } from '../../services/api.jsx';

const { Text, Title } = Typography;

export const CardItem = ({ record }) => {
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
      <Card
        className={classes.cardItem}
        title={<Title level={5}>{record.rank}</Title>}
      >
        <div className={classes.cardContent}>
          <Text strong>Difficulty</Text>
          <Text className={classes.cardText}>{record.difficultyName}</Text>

          <Divider className={classes.cardDivider} />

          <Text strong>Shot Type</Text>
          <Text className={classes.cardText}>{shotType}</Text>

          <Divider className={classes.cardDivider} />

          <Text strong>Clear Date</Text>
          <Text>{record.date}</Text>

          {(record.videoUrl || record.hasReplayFile) && (
            <>
              <Divider className={classes.cardDivider} />
              <Space wrap>
                {record.videoUrl && (
                  <Button
                    icon={<PlayCircleOutlined />}
                    onClick={handleWatchVideo}
                    className={classes.actionButton}
                  >
                    YouTube
                  </Button>
                )}
                {record.hasReplayFile && (
                  <Button
                    icon={<DownloadOutlined />}
                    onClick={handleDownload}
                    loading={downloading}
                    className={classes.downloadButton}
                  >
                    Replay File
                  </Button>
                )}
              </Space>
            </>
          )}
        </div>
      </Card>
    );
  };