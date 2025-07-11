const API_BASE = '';

export const fetchGames = async () => {
    const response = await fetch(`${API_BASE}/Game`);
    return response.json();
}

export const fetchRecords = async (gameId) => {
    const response = await fetch(`${API_BASE}/Record/${gameId}`);
    return response.json();
}

export const fetchDownloadLink = async (recordId) => {
    const response = await fetch(`${API_BASE}/ReplayFile/download/${recordId}`);
    if (!response.ok) {
        throw new Error(response.status === 404
            ? 'Replay file not found!'
            : 'Download failed!');
    }
    return response;
}

export const fetchPagedRecords = async (page = 1, pageSize = 20) => {
    const response = await fetch(`${API_BASE}/Record/paged/${page},${pageSize}`);
    return response.json();
}