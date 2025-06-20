export async function fetchStudyEntries() {
    const response = await fetch('http://localhost:5000/api/study');
    return await response.json();
}

export async function fetchFeatureToggles() {
    const response = await fetch('http://localhost:5000/api/featuretoggles');
    if (!response.ok) {
        throw new Error('Failed to fetch feature toggles');
    }
    return await response.json();
}
