import React, { useState, useEffect } from 'react';
import { 
  Table, Button, Modal, Form, Input, Select, DatePicker, 
  Spin, Alert, Popconfirm, Upload, message, ConfigProvider, theme, Space 
} from 'antd';
import { 
  fetchPagedRecords, 
  addRecord, 
  updateRecord, 
  deleteRecord,
  uploadReplayFile,
  deleteReplayFile,
  fetchGames,
  fetchDifficulties,
  fetchShotTypes
} from '../../services/api';
import dayjs from 'dayjs';
import classes from './AdminPanel.module.css';

const { Option } = Select;

export const RecordAdmin = () => {
  const [records, setRecords] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [modalVisible, setModalVisible] = useState(false);
  const [editingRecord, setEditingRecord] = useState(null);
  const [form] = Form.useForm();
  const [games, setGames] = useState([]);
  const [difficulties, setDifficulties] = useState([]);
  const [shotTypes, setShotTypes] = useState([]);
  const [pagination, setPagination] = useState({
    current: 1,
    pageSize: 10,
    total: 0
  });
  const [uploading, setUploading] = useState({});

  useEffect(() => {
    loadData();
    loadReferenceData();
  }, [pagination.current, pagination.pageSize]);

  const loadData = async () => {
    setLoading(true);
    try {
      const data = await fetchPagedRecords(pagination.current, pagination.pageSize);
      setRecords(data.records);
      setPagination({
        ...pagination,
        total: data.totalCount
      });
    } catch (err) {
        setError(err.message);
    } finally {
        setLoading(false);
    }
  };

  const loadReferenceData = async () => {
    try {
      const [gameData, difficultiesData, shotTypesData] = await Promise.all([
        fetchGames(),
        fetchDifficulties(),
        fetchShotTypes()
      ]);
      setGames(gameData);
      setDifficulties(difficultiesData);
      setShotTypes(shotTypesData);
    } catch (err) {
        setError(err.message);
    }
  };

  const handleTableChange = (pagination) => {
    setPagination(pagination);
  };
 
  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      values.date = values.date.format('YYYY-MM-DD');

    if (editingRecord) {
        await updateRecord(editingRecord.id, values);
      } else {
        await addRecord(values);
      }
      
      setModalVisible(false);
      form.resetFields();
      setEditingRecord(null);
      loadData();
    } catch (err) {
      setError(err.message);
    }
  };

  const handleEdit = (record) => {
    setEditingRecord(record);
    form.setFieldsValue({
      rank: record.rank,
      gameId: record.gameId,
      difficultyId: record.difficultyId,
      shotTypeId: record.shotTypeId,
      date: dayjs(record.date),
      videoUrl: record.videoUrl
    });
    setModalVisible(true);
  };

  const handleDelete = async (id) => {
    try {
      await deleteRecord(id);
      loadData();
    } catch (err) {
      setError(err.message);
    }
  };

  const handleUploadReplay = async (recordId, file) => {
    setUploading({...uploading, [recordId]: true});
    try {
      const formData = new FormData();
      formData.append('recordId', recordId);
      formData.append('file', file);

      await uploadReplayFile(formData);
      message.success('Replay file uploaded successfully!');
      loadData();
    } catch (err) {
      setError(err.message);
    } finally {
      setUploading({...uploading, [recordId]: false});
    }
  };

  const handleDeleteReplay = async (recordId) => {
    try {
      await deleteReplayFile(recordId);
      message.success('Replay file deleted successfully!');
      loadData();
    } catch (err) {
      message.error(`Delete failed: ${err.message}`);
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
      title: 'Game',
      key: 'game',
      render: (_, record) => record.gameName
    },
    {
      title: 'Rank',
      dataIndex: 'rank',
      key: 'rank'
    },
    {
      title: 'Difficulty',
      key: 'difficulty',
      render: (_, record) => record.difficultyName
    },
    {
      title: 'Shot Type',
      key: 'shotType',
      render: (_, record) => `${record.characterName} ${record.shotName}`
    },
    {
      title: 'Date',
      dataIndex: 'date',
      key: 'date'
    },
     {
      title: 'Replay',
      key: 'replay',
      render: (_, record) => (
        record.hasReplayFile ? (
          <Popconfirm
            title="Delete this replay file?"
            onConfirm={() => handleDeleteReplay(record.id)}
            okText="Yes"
            cancelText="No"
          >
            <Button type="primary" danger>
              Delete Replay
            </Button>
          </Popconfirm>
        ) : (
          <Upload
            showUploadList={false}
            beforeUpload={(file) => {
              handleUploadReplay(record.id, file);
              return false;
            }}
          >
            <Button 
              type="primary"
              loading={uploading[record.id]}
            >
              Upload Replay
            </Button>
          </Upload>
        )
      )
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
            title="Delete this record?"
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
      <div style={{ marginBottom: 16 }}>
        <Button type="primary" onClick={() => setModalVisible(true)}>
          Add Record
        </Button>
      </div>

      {error && <Alert message={error} type="error" showIcon style={{ marginBottom: 16 }} />}
      
      <Spin spinning={loading}>
        <Table 
          dataSource={records} 
          columns={columns} 
          rowKey="id"
          pagination={pagination}
          onChange={handleTableChange}
          className={classes.compactTable}
          bordered
        />
      </Spin>

      <Modal
        title={editingRecord ? "Edit Record" : "Add Record"}
        open={modalVisible}
        onOk={handleSubmit}
        onCancel={() => {
          setModalVisible(false);
          form.resetFields();
          setEditingRecord(null);
        }}
        destroyOnHidden
        width={600}
      >
        <Form form={form} layout="vertical">
          <Form.Item
            name="rank"
            label="Rank"
            rules={[{ required: true, message: 'Please enter rank' }]}
          >
            <Input />
          </Form.Item>
          
          <Form.Item
            name="gameId"
            label="Game"
            rules={[{ required: true, message: 'Please select game' }]}
          >
            <Select>
              {games.map(game => (
                <Option key={game.id} value={game.id}>
                  {game.gameName}
                </Option>
              ))}
            </Select>
          </Form.Item>
          
          <Form.Item
            name="difficultyId"
            label="Difficulty"
            rules={[{ required: true, message: 'Please select difficulty' }]}
          >
            <Select>
              {difficulties.map(difficulty => (
                <Option key={difficulty.id} value={difficulty.id}>
                  {difficulty.difficultyName}
                </Option>
              ))}
            </Select>
          </Form.Item>
          
          <Form.Item
            name="shotTypeId"
            label="Shot Type"
            rules={[{ required: true, message: 'Please select shot type' }]}
          >
            <Select>
              {shotTypes.map(shotType => (
                <Option key={shotType.id} value={shotType.id}>
                  {shotType.characterName} {shotType.shotName}
                </Option>
              ))}
            </Select>
          </Form.Item>
          
          <Form.Item
            name="date"
            label="Clear Date"
            rules={[{ 
              required: true, 
              message: 'Please select date' 
            }]}
          >
            <DatePicker 
              disabledDate={(current) => current && current > dayjs().endOf('day')}
              style={{ width: '100%' }}
            />
          </Form.Item>
          
          <Form.Item
            name="videoUrl"
            label="Video URL"
          >
            <Input />
          </Form.Item>
        </Form>
      </Modal>
    </ConfigProvider>
  );
}