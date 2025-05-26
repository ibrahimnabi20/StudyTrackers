import { useState, useEffect } from 'react';
import './App.css';

function App() {
    const [subject, setSubject] = useState('');
    const [duration, setDuration] = useState('');
    const [tag, setTag] = useState('');
    const [color, setColor] = useState('#000000');
    const [entries, setEntries] = useState([]);

    const fetchEntries = async () => {
        const res = await fetch('http://localhost:5000/api/study');
        const data = await res.json();
        setEntries(data);
    };

    const handleAdd = async () => {
        if (!subject || !duration) return;

        const newEntry = {
            subject,
            durationInMinutes: parseInt(duration),
            tag,
            color
        };

        await fetch('http://localhost:5000/api/study', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(newEntry)
        });

        setSubject('');
        setDuration('');
        setTag('');
        setColor('#000000');
        fetchEntries();
    };

    useEffect(() => {
        fetchEntries();
    }, []);

    return (
        <div className="container mt-5">
            <h1 className="text-center">StudyTracker</h1>
            <p className="text-center text-muted mb-4">Track your study sessions with ease</p>

            <div className="card p-4">
                <input
                    className="form-control mb-2"
                    placeholder="Subject"
                    value={subject}
                    onChange={(e) => setSubject(e.target.value)}
                />
                <input
                    className="form-control mb-2"
                    placeholder="Duration (min)"
                    value={duration}
                    type="number"
                    onChange={(e) => setDuration(e.target.value)}
                />
                <input
                    className="form-control mb-2"
                    placeholder="Tag (optional)"
                    value={tag}
                    onChange={(e) => setTag(e.target.value)}
                />
                <input
                    className="form-control mb-3"
                    type="color"
                    value={color}
                    onChange={(e) => setColor(e.target.value)}
                />
                <button className="btn btn-primary w-100" onClick={handleAdd}>Add Entry</button>
            </div>

            <h3 className="mt-5">Your Sessions</h3>
            <ul className="list-group mt-3">
                {entries.length === 0 ? (
                    <p className="text-muted">No entries yet...</p>
                ) : (
                    entries.map((entry, index) => (
                        <li
                            className="list-group-item d-flex justify-content-between align-items-center"
                            key={index}
                            style={{ borderLeft: `8px solid ${entry.color}` }}
                        >
                            <div>
                                <strong>{entry.subject}</strong> – {entry.durationInMinutes} min
                                {entry.tag && <span className="badge bg-secondary ms-2">{entry.tag}</span>}
                            </div>
                            <div>{new Date(entry.timestamp).toLocaleString()}</div>
                        </li>
                    ))
                )}
            </ul>
        </div>
    );
}

export default App;
