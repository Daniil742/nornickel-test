import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthContext } from './context/AuthContext';
import { AuthProvider } from './context/AuthContext';
import Layout from './components/Layout';
import LoginPage from './pages/LoginPage';
import CandidatesPage from './pages/CandidatesPage';
import AnalyticsPage from './pages/AnalyticsPage';
import { useContext, type ReactNode } from 'react';

const PrivateRoute = ({ children }: { children: ReactNode }) => {
    const { user } = useContext(AuthContext);
    return user ? <>{children}</> : <Navigate to="/login" />;
};

function App() {
    return (
        <AuthProvider>
            <BrowserRouter>
                <Routes>
                    <Route path="/login" element={<LoginPage />} />
                    <Route path="/" element={<PrivateRoute><Layout /></PrivateRoute>}>
                        <Route index element={<Navigate to="/candidates" replace />} />
                        <Route path="candidates" element={<CandidatesPage />} />
                        <Route path="analytics" element={<AnalyticsPage />} />
                    </Route>
                </Routes>
            </BrowserRouter>
        </AuthProvider>
    );
}

export default App;