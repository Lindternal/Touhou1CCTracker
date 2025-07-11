import classes from './Header.module.css'

export function Header() {
  return(
    <>
      <header className={classes.header}>
        <h1 className={classes.headerText}>Touhou 1CC Tracker</h1>
      </header>
    </>
  )
}