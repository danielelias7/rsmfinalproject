import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import ReportForm from './ReportForm'

function App() {
  const [count, setCount] = useState(0)

  return (
    <>
      <h1>RSM Final Project</h1>
      <h2>Sales Report</h2>
      <ReportForm />
    </>
  )
}

export default App
