import React, { useEffect, useState } from 'react';
import StudyForm from './components/StudyForm';
import { fetchStudyEntries, fetchFeatureToggles } from './api';

function App() {
    const [entries, setEntries] = useState([]);
    const [toggles, setToggles] = useState({
        enableStudyStats: false
    });

    useEffect(() => {
        fetchStudyEntries().then(data => setEntries(data));
        fetchFeatureToggles().then(data => {
            setToggles({
                enableStudyStats: data.enableStudyStats
            });
            console.log('Feature toggles:', data);
        });
    }, []);

    const totalMinutes = entries.reduce((sum, e) => sum + e.durationInMinutes, 0);

    return (
        <div className="container py-5">
            <div className="text-center mb-5">
                <h1 className="display-4 fw-bold">StudyTracker</h1>
                <p className="text-muted">Track your study sessions with ease</p>
            </div>

            <StudyForm onNewEntry={entry => setEntries([...entries, entry])} />

            {toggles.enableStudyStats && (
                <div className="alert alert-info mt-4">
                    <strong>Stats:</strong> You have studied for a total of {totalMinutes} minutes.
                </div>
            )}

            <h2 className="h4 mt-5 mb-3">Your Sessions</h2>
            {entries.length === 0 ? (
                <p className="text-muted">No entries yet...</p>
            ) : (
                <ul className="list-group" data-testid="entry-list">
                    {entries.map(entry => (
                        <li key={entry.id} data-testid="study-entry" className="list-group-item">
                            <strong>{entry.subject}</strong> — {entry.durationInMinutes} minutes
                            <span className="text-muted float-end">
                                {new Date(entry.timestamp).toLocaleString()}
                            </span>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    );
}

export default App;
