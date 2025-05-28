import React, { useEffect, useState } from 'react'
import StudyForm from './components/StudyForm'
import { fetchStudyEntries } from './api'

function App() {
    const [entries, setEntries] = useState([])

    useEffect(() => {
        fetchStudyEntries().then(data => setEntries(data))
    }, [])

    return (
        <div className="container py-5">
            <div className="text-center mb-5">
                <h1 className="display-4 fw-bold">StudyTracker</h1>
                <p className="text-muted">Track your study sessions with ease</p>
            </div>

            <StudyForm onNewEntry={entry => setEntries([...entries, entry])} />

            <h2 className="h4 mt-5 mb-3">Your Sessions</h2>
            {entries.length === 0 ? (
                <p className="text-muted">No entries yet...</p>
            ) : (
                <ul className="list-group">
                    {entries.map(entry => (
                        <li
                            key={entry.id}
                            className="list-group-item"
                            data-testid="study-entry"
                        >
                            <strong>{entry.subject}</strong> — {entry.durationInMinutes} minutes
                            <span className="text-muted float-end">
        {new Date(entry.timestamp).toLocaleString()}
      </span>
                        </li>
                    ))}
                </ul>

            )}
        </div>
    )
}

export default App
