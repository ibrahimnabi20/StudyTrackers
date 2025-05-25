export async function fetchStudyEntries() {
    const response = await fetch('http://localhost:5000/api/study')
    return await response.json()
}
