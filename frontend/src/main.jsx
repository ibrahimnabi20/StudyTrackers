import React from 'react'
import ReactDOM from 'react-dom/client'
import './index.css'

// Import main React component
import App from './App.jsx'

// Import Tailwind CSS
import './index.css'

// Mount App into the DOM element with id 'root'
ReactDOM.createRoot(document.getElementById('root')).render(
    <React.StrictMode>
        <App />
    </React.StrictMode>
)
