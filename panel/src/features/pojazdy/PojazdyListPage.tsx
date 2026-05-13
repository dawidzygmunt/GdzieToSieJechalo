import { useState } from 'react';
import { Plus } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { DataTable, type Column, Pagination, StatusBadge } from '@/components/common';
import { usePojazdy } from '@/hooks/usePojazdy';
import { usePagination } from '@/hooks/usePagination';
import { AddPojazdDialog } from './AddPojazdDialog';
import type { PojazdListDto } from '@/types/pojazd.types';

const columns: Column<PojazdListDto>[] = [
  { key: 'numerBoczny', header: 'Nr boczny' },
  { key: 'producent', header: 'Producent' },
  { key: 'model', header: 'Model' },
  { key: 'rokProdukcji', header: 'Rok prod.' },
  { key: 'vin', header: 'VIN' },
  {
    key: 'aktywny',
    header: 'Status',
    render: (item) => <StatusBadge status={item.aktywny} />,
  },
];

export function PojazdyListPage() {
  const [isAddDialogOpen, setIsAddDialogOpen] = useState(false);
  const { page, pageSize, setPage, setPageSize } = usePagination();

  const { data, isLoading } = usePojazdy({
    Page: page,
    PageSize: pageSize,
  });

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-900">Pojazdy</h1>
          <p className="text-slate-500 mt-1">Zarządzaj flotą pojazdów</p>
        </div>
        <Button onClick={() => setIsAddDialogOpen(true)}>
          <Plus className="w-4 h-4 mr-2" />
          Dodaj pojazd
        </Button>
      </div>

      <DataTable
        columns={columns}
        data={data?.items ?? []}
        isLoading={isLoading}
        keyExtractor={(item) => item.id}
        emptyMessage="Nie znaleziono pojazdów"
      />

      {data && (
        <Pagination
          currentPage={data.pageNumber}
          totalPages={data.totalPages}
          totalCount={data.totalCount}
          pageSize={pageSize}
          onPageChange={setPage}
          onPageSizeChange={setPageSize}
        />
      )}

      <AddPojazdDialog open={isAddDialogOpen} onOpenChange={setIsAddDialogOpen} />
    </div>
  );
}
