import { Plus } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Card, CardContent } from '@/components/ui/card';

export function KontroleListPage() {
  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-900">Kontrole</h1>
          <p className="text-slate-500 mt-1">Zarządzaj kontrolami biletowymi</p>
        </div>
        <Button>
          <Plus className="w-4 h-4 mr-2" />
          Zapisz kontrolę
        </Button>
      </div>

      <Card>
        <CardContent className="p-6">
          <p className="text-slate-600">
            Tutaj możesz zapisywać przeprowadzone kontrole i wystawiać mandaty.
          </p>
          <p className="text-sm text-slate-500 mt-2">
            Kontrola wymaga: ID kontrolera, ID realizacji kursu, daty/godziny i wyniku.
          </p>
        </CardContent>
      </Card>
    </div>
  );
}
