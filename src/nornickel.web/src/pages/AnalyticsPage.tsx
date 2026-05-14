import { useEffect, useState } from 'react';
import { Box, Grid, Paper, Typography } from '@mui/material';
import { Bar } from 'react-chartjs-2';
import { Chart as ChartJS, CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend } from 'chart.js';
import axiosInstance from '../api/axiosInstance';
import { type ChartDataResponseDto } from '../types/apiTypes';

ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend);

interface ChartVisualData {
    labels: string[];
    datasets: {
        label: string;
        data: number[];
        backgroundColor: string;
    }[];
}

export default function AnalyticsPage() {
    const [salaryDist, setSalaryDist] = useState<ChartVisualData | null>(null);
    const [ageDist, setAgeDist] = useState<ChartVisualData | null>(null);
    const [salaryByExp, setSalaryByExp] = useState<ChartVisualData | null>(null);

    useEffect(() => {
        const fetchAnalytics = async () => {
            const [salRes, ageRes, expRes] = await Promise.all([
                axiosInstance.get<ChartDataResponseDto>('/analytics/salary-distribution'),
                axiosInstance.get<ChartDataResponseDto>('/analytics/age-distribution'),
                axiosInstance.get<ChartDataResponseDto>('/analytics/salary-by-experience')
            ]);

            const mapToChartData = (dto: ChartDataResponseDto, label: string, color: string): ChartVisualData => ({
                labels: dto.labels,
                datasets: [{ label, data: dto.values, backgroundColor: color }]
            });

            setSalaryDist(mapToChartData(salRes.data, 'Кол-во кандидатов', 'rgba(54, 162, 235, 0.6)'));
            setAgeDist(mapToChartData(ageRes.data, 'Кол-во кандидатов', 'rgba(75, 192, 192, 0.6)'));
            setSalaryByExp(mapToChartData(expRes.data, 'Средняя ЗП (₽)', 'rgba(255, 99, 132, 0.6)'));
        };
        fetchAnalytics();
    }, []);

    const options = (title: string) => ({ responsive: true, plugins: { title: { display: true, text: title } } });

    return (
        <Box>
            <Typography variant="h4" gutterBottom>Аналитика кандидатов</Typography>
            <Grid container spacing={3}>
                <Grid item xs={12} md={4}>
                    <Paper sx={{ p: 3 }}>
                        {salaryDist && <Bar data={salaryDist} options={options('Распределение по ЗП')} />}
                    </Paper>
                </Grid>
                <Grid item xs={12} md={4}>
                    <Paper sx={{ p: 3 }}>
                        {ageDist && <Bar data={ageDist} options={options('Распределение по возрасту')} />}
                    </Paper>
                </Grid>
                <Grid item xs={12} md={4}>
                    <Paper sx={{ p: 3 }}>
                        {salaryByExp && <Bar data={salaryByExp} options={options('Средняя ЗП от Опыта')} />}
                    </Paper>
                </Grid>
            </Grid>
        </Box>
    );
}