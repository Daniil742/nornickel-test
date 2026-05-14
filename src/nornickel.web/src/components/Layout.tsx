import { useContext, ReactNode } from 'react';
import { Outlet, useNavigate } from 'react-router-dom';
import { AppBar, Toolbar, Typography, Button, Box } from '@mui/material';
import LogoutIcon from '@mui/icons-material/Logout';
import { AuthContext } from '../context/AuthContext';

export default function Layout() {
    const { user, logout } = useContext(AuthContext);
    const navigate = useNavigate();

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    return (
        <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
            <AppBar position="static">
                <Toolbar sx={{ display: 'flex', justifyContent: 'space-between' }}>
                    <Typography variant="h6">HR System</Typography>
                    {user && (
                        <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                            <Typography>{user.username} ({user.role})</Typography>
                            <Button color="inherit" startIcon={<LogoutIcon />} onClick={handleLogout}>
                                Выйти
                            </Button>
                        </Box>
                    )}
                </Toolbar>
            </AppBar>
            <Box sx={{ p: 3, flexGrow: 1 }}>
                <Outlet />
            </Box>
        </Box>
    );
}