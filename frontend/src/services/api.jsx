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

export const login = async (credentials) => {
    const response = await fetch(`${API_BASE}/Auth/login`, {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(credentials),
        credentials: 'include'
    });
    
    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || 'Login failed')
    }

    return response.json();
}

export const register = async (credentials) => {
    const response = await fetch(`${API_BASE}/Auth/register`, {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        body: JSON.stringify(credentials),
        credentials: 'include'
    });

    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || 'Registration failed')
    }

    return {};
}

export const logout = async () => {
    await fetch(`${API_BASE}/Auth/logout`, {
        method: 'POST',
        credentials: 'include'
    });
}

export const checkAuth = async () => {
    const response = await fetch(`${API_BASE}/Auth/check`, {
        credentials: 'include'
    });
    return response.ok ? response.json() : null;
};