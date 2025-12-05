// Example: Frontend API integration for login and character CRUD
const API_BASE = 'http://localhost:5066/api'; // Updated to match backend port
let jwtToken = '';

// Login function
async function login(username, password) {
    const res = await fetch(`${API_BASE}/auth/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, passwordHash: password })
    });
    const data = await res.json();
    if (res.ok && data.token) {
        jwtToken = data.token;
        localStorage.setItem('jwtToken', jwtToken);
        alert('Login successful!');
    } else {
        alert(data.error || 'Login failed');
    }
}

// Get all characters
async function getCharacters() {
    const res = await fetch(`${API_BASE}/characters`, {
        headers: { 'Authorization': `Bearer ${jwtToken}` }
    });
    return await res.json();
}

// Create a character
async function createCharacter(name, level, userId) {
    const res = await fetch(`${API_BASE}/characters`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${jwtToken}`
        },
        body: JSON.stringify({ name, level, userId })
    });
    return await res.json();
}

// Update a character
async function updateCharacter(id, name, level, userId) {
    const res = await fetch(`${API_BASE}/characters/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${jwtToken}`
        },
        body: JSON.stringify({ id, name, level, userId })
    });
    return res.status === 204;
}

// Delete a character
async function deleteCharacter(id) {
    const res = await fetch(`${API_BASE}/characters/${id}`, {
        method: 'DELETE',
        headers: { 'Authorization': `Bearer ${jwtToken}` }
    });
    return res.status === 204;
}

// Usage example:
// login('admin', 'adminhash');
// getCharacters().then(console.log);
// createCharacter('Hero', 1, 1).then(console.log);
// updateCharacter(1, 'Hero', 2, 1);
// deleteCharacter(1);
