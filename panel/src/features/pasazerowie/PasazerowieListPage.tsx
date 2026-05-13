import { Plus } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Card, CardContent } from '@/components/ui/card';

export function PasazerowieListPage() {
  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-900">Pasażerowie</h1>
          <p className="text-slate-500 mt-1">Zarządzaj pasażerami i biletami</p>
        </div>
        <Button>
          <Plus className="w-4 h-4 mr-2" />
          Dodaj pasażera
        </Button>
      </div>

      <Card>
        <CardContent className="p-6">
          <p className="text-slate-600">
            Tutaj możesz dodawać pasażerów i wystawiać im bilety okresowe.
          </p>
          <p className="text-sm text-slate-500 mt-2">
            Pasażer wymaga: imię, nazwisko, opcjonalnie PESEL i email.
          </p>
        </CardContent>
      </Card>
    </div>
  );
}
