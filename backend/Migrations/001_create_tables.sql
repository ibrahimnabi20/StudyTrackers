CREATE TABLE IF NOT EXISTS StudyEntries (
                                            Id INT AUTO_INCREMENT PRIMARY KEY,
                                            Subject VARCHAR(255) NOT NULL,
    DurationInMinutes INT NOT NULL,
    Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP
    );
