import { Plus } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Card, CardContent } from '@/components/ui/card';

export function GrafikiListPage() {
  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-900">Grafiki pracy</h1>
          <p className="text-slate-500 mt-1">Zarządzaj grafikami pracy kierowców</p>
        </div>
        <Button>
          <Plus className="w-4 h-4 mr-2" />
          Dodaj wpis
        </Button>
      </div>

      <Card>
        <CardContent className="p-6">
          <p className="text-slate-600">
            Tutaj możesz dodawać wpisy do grafików pracy kierowców.
          </p>
          <p className="text-sm text-slate-500 mt-2">
            Każdy wpis zawiera: kierowcę, pojazd, datę oraz godziny pracy (od-do).
          </p>
        </CardContent>
      </Card>
    </div>
  );
}
