import { useState } from 'react';
import './App.css';

function App() {
    const [subject, setSubject] = useState('');
    const [duration, setDuration] = useState('');
    const [entries, setEntries] = useState([]);

    const handleAdd = () => {
        if (!subject || !duration) return;

        const newEntry = {
            subject,
            duration: parseInt(duration),
            timestamp: new Date().toISOString()
        };

        setEntries([...entries, newEntry]);
        setSubject('');
        setDuration('');
    };

    return (
        <div className="container mt-5">
            <h1 className="text-center">StudyTracker</h1>
            <p className="text-center text-muted mb-4">Track your study sessions with ease</p>

            <div className="card p-4">
                <input
                    className="form-control mb-2"
                    placeholder="Subject" // here
                    value={subject}
                    onChange={(e) => setSubject(e.target.value)}
                />
                <input
                    className="form-control mb-3"
                    placeholder="Duration (min)" // and here 
                    value={duration}
                    onChange={(e) => setDuration(e.target.value)}
                    type="number"
                />
                <button className="btn btn-primary w-100" onClick={handleAdd}>Add Entry</button>
            </div>

            <h3 className="mt-5">Your Sessions</h3>
            <ul className="list-group mt-3">
                {entries.length === 0 ? (
                    <p className="text-muted">No entries yet...</p>
                ) : (
                    entries.map((entry, index) => (
                        <li className="list-group-item" key={index}>
                            <strong>{entry.subject}</strong> – {entry.duration} min
                        </li>
                    ))
                )}
            </ul>
        </div>
    );
}

export default App;
