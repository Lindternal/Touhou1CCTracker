import React, { useEffect, useState } from 'react';
import { ConfigProvider, Tabs, Spin, Alert, Pagination  } from 'antd';
import classes from './MainMenu.module.css'
import { CardContainer } from '../CardContainer/CardContainer.jsx';
import { RecentCardItem } from '../RecentCardItem/RecentCardItem.jsx';
import { fetchGames, fetchPagedRecords, fetchRecords } from '../../services/api.jsx';

export function MainMenu({ user }) {
  const [games, setGames] = useState([]);
  const [recordsByGame, setRecordsByGame] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [recentRecords, setRecentRecords] = useState([]);
  const [recentLoading, setRecentLoading] = useState(true);
  const [recentError, setRecentError] = useState(null);
  const [pagination, setPagination] = useState({
    current: 1,
    pageSize: 20,
    total: 0
  });

  const loadData = async () => {
    try {
      const gamesData = await fetchGames();
      setGames(gamesData);

      const recordsPromises = gamesData.map(game =>
        fetchRecords(game.id).catch(e => [])
      );

      const recordsResults = await Promise.all(recordsPromises);

      const recordsMap = {};
      gamesData.forEach((game, index) => {
        recordsMap[game.id] = recordsResults[index] || [];
      });

      setRecordsByGame(recordsMap);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const loadRecentRecords = async (page, pageSize) => {
    try {
      const data = await fetchPagedRecords(page, pageSize);
      setRecentRecords(data.records);
      setPagination({
        current: data.currentPage,
        pageSize: data.pageSize,
        total: data.totalCount
      });
    } catch (err) {
      setRecentError(err.message);
    } finally {
      setRecentLoading(false);
    }
  };

  useEffect(() => {
    loadData();
    loadRecentRecords(pagination.current, pagination.pageSize);
  }, [pagination.current, pagination.pageSize]);

  const allRecordsTabContent = (
    <>
      {games.filter(game => recordsByGame[game.id]?.length > 0).map(game => (
        <CardContainer
          key = { game.id }
          title = { game.name }
          records = { recordsByGame[game.id] }
        />
      ))}
    </>
  );

  const recentTabContent = (
    <>
      <div className={classes.recentContainer}>
        {recentRecords.map(record => (
          <RecentCardItem key={record.id} record={record} />
        ))}
      </div>
      {pagination.total > pagination.pageSize && (
        <Pagination
          current={pagination.current}
          pageSize={pagination.pageSize}
          total={pagination.total}
          onChange={(page, pageSize) => setPagination(prev => ({...prev, current: page, pageSize}))}
          className={classes.pagination}
          showSizeChanger={false}
        />
      )}
    </>
  );

  const adminTab = {
    key: '3',
    label: 'Admin Panel',
    children: (
      <div className={classes.adminPanel}>
        <h2>SOMETHING HERE</h2>
      </div>
    )
  };

  const items = [
    {
      key: '1',
      label: 'All Records',
      children: loading ? (
        <Spin size="large" className={classes.spinner} />
      ) : error ? (
        <Alert message="Data load error" description={error} type="error" className={classes.alert} />
      ) : recentRecords.length === 0 ? (
        <div className={classes.noRecords}>No Records available!</div>
      ) : (
        allRecordsTabContent
      )
    },
    {
      key: '2',
      label: 'Recent',
      children: recentLoading ? (
        <Spin size="large" className={classes.spinner} />
      ) : recentError ? (
        <Alert message="Recent data load error" description={error} type="error" className={classes.alert} />
      ) : recentRecords.length === 0 ? (
        <div className={classes.noRecords}>No Records available!</div>
      ) : (
        recentTabContent
      )
    }
  ];

  if (user && user.role === 'Admin') {
    items.push(adminTab);
  }

  return(
    <>
      <ConfigProvider
        theme={{
          components: {
            Tabs: {
              itemColor: 'rgba(245, 245, 245, 0.85)',
              itemSelectedColor: 'rgb(87, 104, 255)',
              itemHoverColor: 'rgb(167, 176, 255)',
              inkBarColor: 'rgb(87, 104, 255)',
              colorBorderSecondary: 'rgba(20, 20, 20, 0)',
              fontSize: 28,
            },
          },
        }}
      >
        <Tabs
          defaultActiveKey='1'
          items={items}
          centered
          className={classes.styles}
        />
      </ConfigProvider>
    </>
  )
}