const API_URL = "http://localhost:5000/api";

function getToken() {
    return localStorage.getItem("jwtToken");
}

function getUser() {
    const token = getToken();
    if (!token) return null;
    try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        const roleKey = Object.keys(payload).find(k => k.endsWith('role'));
        const nameidKey = Object.keys(payload).find(k => k.endsWith('nameidentifier'));
        const emailKey = Object.keys(payload).find(k => k.endsWith('emailaddress'));
        
        return {
            userId: payload[nameidKey],
            email: payload[emailKey],
            role: payload[roleKey]
        };
    } catch (e) {
        return null;
    }
}

function setupNavigation() {
    const user = getUser();
    const navLinks = $('#nav-links');
    navLinks.empty();
    
    if (user) {
        $('#logout-btn-container').removeClass('d-none');
        if (user.role === 'Admin') {
            navLinks.append('<li class="nav-item"><a class="nav-link text-light" href="/Admin/Dashboard">Admin Dashboard</a></li>');
            navLinks.append('<li class="nav-item"><a class="nav-link text-light" href="/Admin/Users">Manage Users</a></li>');
            navLinks.append('<li class="nav-item"><a class="nav-link text-light" href="/Admin/LeaveTypes">Leave Types</a></li>');
            navLinks.append('<li class="nav-item"><a class="nav-link text-light" href="/Admin/Holidays">Holidays</a></li>');
            navLinks.append('<li class="nav-item"><a class="nav-link text-light" href="/Admin/LeaveRequests">Leave Requests</a></li>');
        } else {
            navLinks.append('<li class="nav-item"><a class="nav-link text-light" href="/Employee/Dashboard">Dashboard</a></li>');
            navLinks.append('<li class="nav-item"><a class="nav-link text-light" href="/Employee/ApplyLeave">Apply Leave</a></li>');
            navLinks.append('<li class="nav-item"><a class="nav-link text-light" href="/Employee/LeaveHistory">History</a></li>');
        }
    } else {
        navLinks.append('<li class="nav-item"><a class="nav-link text-light" href="/Home/Login">Login</a></li>');
        $('#logout-btn-container').addClass('d-none');
    }
}

$(document).ready(function() {
    setupNavigation();
    
    $('#logoutBtn').click(function() {
        localStorage.removeItem("jwtToken");
        window.location.href = '/Home/Login';
    });
});

function apiCall(endpoint, method, data, onSuccess, onError) {
    const headers = {
        'Content-Type': 'application/json'
    };
    const token = getToken();
    if (token) {
        headers['Authorization'] = 'Bearer ' + token;
    }

    $.ajax({
        url: API_URL + endpoint,
        type: method,
        headers: headers,
        data: data ? JSON.stringify(data) : null,
        success: function(res) {
            if (onSuccess) onSuccess(res);
        },
        error: function(err) {
            if (err.status === 401 && endpoint !== '/Auth/login') {
                localStorage.removeItem("jwtToken");
                window.location.href = '/Home/Login';
            }
            if (onError) onError(err);
        }
    });
}
