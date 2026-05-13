import { MapPin, Route, Bus, Users, Calendar, ClipboardCheck, UserCheck, Ticket } from 'lucide-react';
import { Card, CardContent } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { useNavigate } from 'react-router-dom';

interface StatCardProps {
  title: string;
  icon: React.ReactNode;
  href: string;
}

function StatCard({ title, icon, href }: StatCardProps) {
  const navigate = useNavigate();

  return (
    <Card
      className="cursor-pointer hover:shadow-md transition-shadow"
      onClick={() => navigate(href)}
    >
      <CardContent className="p-6">
        <div className="flex items-center gap-4">
          <div className="w-12 h-12 bg-primary-100 rounded-lg flex items-center justify-center">
            {icon}
          </div>
          <div>
            <p className="text-sm text-slate-500">Zarządzaj</p>
            <p className="text-lg font-semibold text-slate-900">{title}</p>
          </div>
        </div>
      </CardContent>
    </Card>
  );
}

export function DashboardPage() {
  const navigate = useNavigate();

  const modules = [
    { title: 'Przystanki', icon: <MapPin className="w-6 h-6 text-primary-600" />, href: '/przystanki' },
    { title: 'Linie', icon: <Route className="w-6 h-6 text-primary-600" />, href: '/linie' },
    { title: 'Pojazdy', icon: <Bus className="w-6 h-6 text-primary-600" />, href: '/pojazdy' },
    { title: 'Kierowcy', icon: <Users className="w-6 h-6 text-primary-600" />, href: '/kierowcy' },
    { title: 'Realizacje kursów', icon: <ClipboardCheck className="w-6 h-6 text-primary-600" />, href: '/realizacje' },
    { title: 'Grafiki pracy', icon: <Calendar className="w-6 h-6 text-primary-600" />, href: '/grafiki' },
    { title: 'Kontrole', icon: <UserCheck className="w-6 h-6 text-primary-600" />, href: '/kontrole' },
    { title: 'Pasażerowie', icon: <Ticket className="w-6 h-6 text-primary-600" />, href: '/pasazerowie' },
  ];

  return (
    <div className="space-y-6">
      {/* Header */}
      <div>
        <h1 className="text-2xl font-bold text-slate-900">Dashboard</h1>
        <p className="text-slate-500 mt-1">Witaj w panelu zarządzania komunikacją miejską</p>
      </div>

      {/* Quick stats grid */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        {modules.map((module) => (
          <StatCard key={module.href} {...module} />
        ))}
      </div>

      {/* Quick actions */}
      <Card>
        <CardContent className="p-6">
          <h2 className="text-lg font-semibold text-slate-900 mb-4">Szybkie akcje</h2>
          <div className="flex flex-wrap gap-3">
            <Button onClick={() => navigate('/przystanki')} variant="outline">
              <MapPin className="w-4 h-4 mr-2" />
              Dodaj przystanek
            </Button>
            <Button onClick={() => navigate('/linie')} variant="outline">
              <Route className="w-4 h-4 mr-2" />
              Dodaj linię
            </Button>
            <Button onClick={() => navigate('/pojazdy')} variant="outline">
              <Bus className="w-4 h-4 mr-2" />
              Dodaj pojazd
            </Button>
            <Button onClick={() => navigate('/kierowcy')} variant="outline">
              <Users className="w-4 h-4 mr-2" />
              Dodaj kierowcę
            </Button>
          </div>
        </CardContent>
      </Card>

      {/* Info */}
      <Card>
        <CardContent className="p-6">
          <h2 className="text-lg font-semibold text-slate-900 mb-2">Informacje o systemie</h2>
          <p className="text-sm text-slate-600">
            GdzieToSięJechało - System zarządzania komunikacją miejską.
            Panel pozwala na zarządzanie przystankami, liniami, pojazdami, kierowcami,
            realizacjami kursów, grafikami pracy, kontrolami i pasażerami.
          </p>
        </CardContent>
      </Card>
    </div>
  );
}
