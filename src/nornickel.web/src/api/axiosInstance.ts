import axios, { type InternalAxiosRequestConfig } from 'axios';

const axiosInstance = axios.create({
    baseURL: 'https://localhost:7217/api/v1.0',
});

axiosInstance.interceptors.request.use((config: InternalAxiosRequestConfig) => {
    const token = localStorage.getItem('token');
    if (token && config.headers) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

export default axiosInstance;