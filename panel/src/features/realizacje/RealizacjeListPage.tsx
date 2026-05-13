import { Plus } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Card, CardContent } from '@/components/ui/card';

export function RealizacjeListPage() {
  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-900">Realizacje kursów</h1>
          <p className="text-slate-500 mt-1">Zarządzaj realizacjami kursów</p>
        </div>
        <Button>
          <Plus className="w-4 h-4 mr-2" />
          Dodaj realizację
        </Button>
      </div>

      <Card>
        <CardContent className="p-6">
          <p className="text-slate-600">
            Tutaj możesz tworzyć nowe realizacje kursów i przypisywać do nich kierowców oraz pojazdy.
          </p>
          <p className="text-sm text-slate-500 mt-2">
            Aby utworzyć realizację, potrzebujesz: ID wariantu trasy, datę kursu i numer kursu.
          </p>
        </CardContent>
      </Card>
    </div>
  );
}
