const API_BASE = '';

const handleApiError = async (response) => {
    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || 'Request Failed!')
    }
    return response;
}

//Games
export const fetchGames = async () => {
    const response = await fetch(`${API_BASE}/Game`);
    await handleApiError(response);
    return response.json();
}

export const addGame = async (game) => {
    const response = await fetch(`${API_BASE}/Game`, {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        credentials: 'include',
        body: JSON.stringify(game)
    })
    await handleApiError(response);
    return response.json();
}

export const updateGame = async (id, game) => {
    const response = await fetch(`${API_BASE}/Game/${id}`, {
        method: 'PUT',
        headers: {'Content-Type': 'application/json'},
        credentials: 'include',
        body: JSON.stringify(game)
    })
    await handleApiError(response);
    return response.json();
}

export const deleteGame = async (id) => {
    const response = await fetch(`${API_BASE}/Game/${id}`, {
        method: 'DELETE',
        credentials: 'include'
    })
    await handleApiError(response);
}

//Difficulty
export const fetchDifficulties = async () => {
    const response = await fetch(`${API_BASE}/Difficulty`, {
        method: 'GET',
        credentials: 'include'
    })
    await handleApiError(response);
    return response.json();
}

export const addDifficulty = async (difficulty) => {
    const response = await fetch(`${API_BASE}/Difficulty`, {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        credentials: 'include',
        body: JSON.stringify(difficulty)
    })
    await handleApiError(response);
    return response.json();
}

export const updateDifficulty = async (id, difficulty) => {
    const response = await fetch(`${API_BASE}/Difficulty/${id}`, {
        method: 'PUT',
        headers: {'Content-Type': 'application/json'},
        credentials: 'include',
        body: JSON.stringify(difficulty)
    })
    await handleApiError(response);
    return response.json();
}

export const deleteDifficulty = async (id) => {
    const response = await fetch(`${API_BASE}/Difficulty/${id}`, {
        method: 'DELETE',
        credentials: 'include'
    })
    await handleApiError(response);
}

//ShotType
export const fetchShotTypes = async () => {
    const response = await fetch(`${API_BASE}/ShotType`, {
        method: 'GET',
        credentials: 'include'
    })
    await handleApiError(response);
    return response.json();
}

export const addShotType = async (shotType) => {
    const response = await fetch(`${API_BASE}/ShotType`, {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        credentials: 'include',
        body: JSON.stringify(shotType)
    })
    await handleApiError(response);
    return response.json();
}

export const updateShotType = async (id, shotType) => {
    const response = await fetch(`${API_BASE}/ShotType/${id}`, {
        method: 'PUT',
        headers: {'Content-Type': 'application/json'},
        credentials: 'include',
        body: JSON.stringify(shotType)
    })
    await handleApiError(response);
    return response.json();
}

export const deleteShotType = async (id) => {
    const response = await fetch(`${API_BASE}/ShotType/${id}`, {
        method: 'DELETE',
        credentials: 'include'
    })
    await handleApiError(response);
}

//Record
export const fetchRecords = async (gameId) => {
    const response = await fetch(`${API_BASE}/Record/${gameId}`);
    return response.json();
}

export const fetchPagedRecords = async (page = 1, pageSize = 20) => {
    const response = await fetch(`${API_BASE}/Record/paged/${page},${pageSize}`);
    return response.json();
}

export const addRecord = async (record) => {
    const response = await fetch(`${API_BASE}/Record`, {
        method: 'POST',
        headers: {'Content-Type': 'application/json'},
        credentials: 'include',
        body: JSON.stringify(record)
    })
    await handleApiError(response);
    return response.json();
}

export const updateRecord = async (id, record) => {
    const response = await fetch(`${API_BASE}/Record/${id}`, {
        method: 'PUT',
        headers: {'Content-Type': 'application/json'},
        credentials: 'include',
        body: JSON.stringify(record)
    })
    await handleApiError(response);
    return response.json();
}

export const deleteRecord = async (id) => {
    const response = await fetch(`${API_BASE}/Record/${id}`, {
        method: 'DELETE',
        credentials: 'include'
    })
    await handleApiError(response);
}

//Replay File
export const fetchDownloadLink = async (recordId) => {
    const response = await fetch(`${API_BASE}/ReplayFile/download/${recordId}`);
    if (!response.ok) {
        throw new Error(response.status === 404
            ? 'Replay file not found!'
            : 'Download failed!');
    }
    return response;
}

export const uploadReplayFile = async (formData) => {
    const response = await fetch(`${API_BASE}/ReplayFile/upload`, {
        method: 'POST',
        credentials: 'include',
        body: formData
    })
    await handleApiError(response);
}

export const deleteReplayFile = async (recordId) => {
    const response = await fetch(`${API_BASE}/ReplayFile/delete/${recordId}`, {
        method: 'DELETE',
        credentials: 'include'
    })
    await handleApiError(response);
}

//Settings
export const fetchSettings = async () => {
    const response = await fetch(`${API_BASE}/Settings`, {
        method: 'GET',
        credentials: 'include'
    })
    await handleApiError(response);
    return response.json();
}

export const updateSetting = async (setting) => {
    const response = await fetch(`${API_BASE}/Settings/settings`, {
        method: 'PUT',
        headers: {'Content-Type': 'application/json'},
        credentials: 'include',
        body: JSON.stringify(setting)
    })
    await handleApiError(response);
}

//Auth
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