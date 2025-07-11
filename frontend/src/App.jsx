import { Footer } from './components/Footer/Footer';
import { Header } from './components/Header/Header';
import { MainMenu } from './components/MainMenu/MainMenu';
import classes from './App.module.css';

export function App() {
  return (
    <div className={classes.appContainer}>
      <Header />
      <main className={classes.mainContent}>
        <MainMenu />
      </main>
      <Footer />
    </div>
  )
}
