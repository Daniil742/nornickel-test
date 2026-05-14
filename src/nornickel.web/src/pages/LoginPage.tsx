import { useState, useContext, type FormEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import { Box, Button, Card, CardContent, TextField, Typography, Alert } from '@mui/material';
import LoginIcon from '@mui/icons-material/Login';
import axiosInstance from '../api/axiosInstance';
import { AuthContext } from '../context/AuthContext';
import { type LoginRequestDto, type LoginResponseDto } from '../types/apiTypes';

export default function LoginPage() {
    const [formData, setFormData] = useState<LoginRequestDto>({ username: '', password: '' });
    const [error, setError] = useState<string>('');
    const { login } = useContext(AuthContext);
    const navigate = useNavigate();

    const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setError('');
        try {
            const response = await axiosInstance.post<LoginResponseDto>('/login', formData);
            login(response.data);
            navigate('/candidates');
        } catch (err: any) {
            if (err.response?.status === 401) {
                setError('Неверный логин или пароль');
            } else {
                setError('Ошибка сервера. Попробуйте позже.');
            }
        }
    };

    return (
        <Box sx={{ display: 'flex', justifyContent: 'center', mt: 10 }}>
            <Card sx={{ minWidth: 350, boxShadow: 3 }}>
                <CardContent>
                    <Typography variant="h5" align="center" gutterBottom>
                        Авторизация
                    </Typography>
                    {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
                    <Box component="form" onSubmit={handleSubmit}>
                        <TextField
                            fullWidth margin="normal" label="Логин" variant="outlined"
                            value={formData.username}
                            onChange={(e) => setFormData({ ...formData, username: e.target.value })}
                            required
                        />
                        <TextField
                            fullWidth margin="normal" label="Пароль" type="password" variant="outlined"
                            value={formData.password}
                            onChange={(e) => setFormData({ ...formData, password: e.target.value })}
                            required
                        />
                        <Button
                            type="submit" fullWidth variant="contained" startIcon={<LoginIcon />}
                            sx={{ mt: 2, mb: 2 }}
                        >
                            Войти
                        </Button>
                    </Box>
                </CardContent>
            </Card>
        </Box>
    );
}