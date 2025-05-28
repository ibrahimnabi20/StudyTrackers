import React, { useState } from 'react';

function StudyForm({ onNewEntry }) {
    const [subject, setSubject] = useState('');
    const [duration, setDuration] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await fetch('/api/study', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    subject,
                    durationInMinutes: parseInt(duration),
                }),
            });

            if (!response.ok) {
                throw new Error('Failed to save entry');
            }

            const data = await response.json();
            onNewEntry(data);
            setSubject('');
            setDuration('');
        } catch (error) {
            console.error('Error adding study entry:', error);
        }
    };

    return (
        <form onSubmit={handleSubmit} className="card card-body shadow-sm">
            <div className="mb-3">
                <label className="form-label">Subject</label>
                <input
                    className="form-control"
                    type="text"
                    value={subject}
                    onChange={e => setSubject(e.target.value)}
                    required
                    placeholder="Subject"
                />
            </div>
            <div className="mb-3">
                <label className="form-label">Duration (minutes)</label>
                <input
                    className="form-control"
                    type="number"
                    value={duration}
                    onChange={e => setDuration(e.target.value)}
                    required
                    placeholder="Duration (minutes)"
                />
            </div>
            <button type="submit" className="btn btn-primary w-100">
                Add Entry
            </button>
        </form>
    );
}

export default StudyForm;
