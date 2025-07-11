import React from 'react';
import { Card, Divider } from 'antd';
import { CardItem } from '../CardItem/CardItem';
import classes from './CardContainer.module.css';

export const CardContainer = ({ title, records = [] }) => {
  if (!records || records.length === 0) {
    return (
      <Card className={classes.cardContainer}>
        <div className={classes.containerContent}>
          <h2>{title}</h2>
          <p>No records available!</p>
        </div>
      </Card>
    );
  }

  return (<Card className={classes.cardContainer}>
    <div className={classes.containerContent}>
      <div className={classes.cardContainerTitle}>
        <h2>{title}</h2>
      </div>
      <div className={classes.cardsArea}>
        {records.map((record, index) => (
          <React.Fragment key={record.id}>
            <div className={classes.cardSlot}>
              <CardItem record={record} />
            </div>
            {index < records.length - 1 && (
              <Divider type="vertical" className={classes.cardDivider} />
            )}
          </React.Fragment>
        ))}
      </div>
    </div>
  </Card>
  );
};