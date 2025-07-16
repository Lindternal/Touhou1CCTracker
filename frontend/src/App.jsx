import { useState, useEffect } from 'react';
import { Footer } from './components/Footer/Footer';
import { Header } from './components/Header/Header';
import { MainMenu } from './components/MainMenu/MainMenu';
import classes from './App.module.css';
import { checkAuth } from './services/api';

export function App() {
  const [user, setUser] = useState(null);

  useEffect(() => {
    const verifyAuth = async () => {
      const userData = await checkAuth();
      if (userData) setUser(userData);
    };

    verifyAuth();
  }, []);

  return (
    <div className={classes.appContainer}>
      <Header user={user} setUser={setUser} />
      <main className={classes.mainContent}>
        <MainMenu user={user} />
      </main>
      <Footer />
    </div>
  )
}
