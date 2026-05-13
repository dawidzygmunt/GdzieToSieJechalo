import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import { AppLayout } from '@/components/layout';
import { ProtectedRoute } from '@/features/auth/ProtectedRoute';
import { LoginPage } from '@/features/auth/LoginPage';
import { DashboardPage } from '@/features/dashboard/DashboardPage';
import { PrzystankiListPage } from '@/features/przystanki/PrzystankiListPage';
import { LinieListPage } from '@/features/linie/LinieListPage';
import { PojazdyListPage } from '@/features/pojazdy/PojazdyListPage';
import { KierowcyListPage } from '@/features/kierowcy/KierowcyListPage';
import { RealizacjeListPage } from '@/features/realizacje/RealizacjeListPage';
import { GrafikiListPage } from '@/features/grafiki/GrafikiListPage';
import { KontroleListPage } from '@/features/kontrole/KontroleListPage';
import { PasazerowieListPage } from '@/features/pasazerowie/PasazerowieListPage';

const router = createBrowserRouter([
  {
    path: '/login',
    element: <LoginPage />,
  },
  {
    element: <ProtectedRoute />,
    children: [
      {
        element: <AppLayout />,
        children: [
          {
            path: '/',
            element: <DashboardPage />,
          },
          {
            path: '/przystanki',
            element: <PrzystankiListPage />,
          },
          {
            path: '/linie',
            element: <LinieListPage />,
          },
          {
            path: '/pojazdy',
            element: <PojazdyListPage />,
          },
          {
            path: '/kierowcy',
            element: <KierowcyListPage />,
          },
          {
            path: '/realizacje',
            element: <RealizacjeListPage />,
          },
          {
            path: '/grafiki',
            element: <GrafikiListPage />,
          },
          {
            path: '/kontrole',
            element: <KontroleListPage />,
          },
          {
            path: '/pasazerowie',
            element: <PasazerowieListPage />,
          },
        ],
      },
    ],
  },
  {
    path: '/unauthorized',
    element: (
      <div className="min-h-screen flex items-center justify-center bg-slate-50">
        <div className="text-center">
          <h1 className="text-2xl font-bold text-slate-900">Brak dostępu</h1>
          <p className="text-slate-600 mt-2">Nie masz uprawnień do tej strony.</p>
        </div>
      </div>
    ),
  },
]);

export function AppRouter() {
  return <RouterProvider router={router} />;
}
