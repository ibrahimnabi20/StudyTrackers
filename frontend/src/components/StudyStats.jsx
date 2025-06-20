const StudyStats = ({ entries }) => {
    if (!entries || entries.length === 0) return <p className="text-muted">No statistics available yet.</p>;

    const total = entries.reduce((acc, curr) => acc + curr.durationInMinutes, 0);
    const average = (total / entries.length).toFixed(2);

    return (
        <div className="alert alert-info mt-4">
            <h5 className="mb-2">Study Statistics</h5>
            <p>Total Sessions: {entries.length}</p>
            <p>Total Time: {total} minutes</p>
            <p>Average Time: {average} minutes</p>
        </div>
    );
};

export default StudyStats;
