import { useState } from 'react';
import { Plus } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { DataTable, type Column, Pagination, SearchInput, StatusBadge } from '@/components/common';
import { usePrzystanki } from '@/hooks/usePrzystanki';
import { usePagination } from '@/hooks/usePagination';
import type { PrzystanekListDto } from '@/types/przystanek.types';

const columns: Column<PrzystanekListDto>[] = [
  { key: 'nazwa', header: 'Nazwa' },
  { key: 'ulica', header: 'Ulica' },
  { key: 'typ', header: 'Typ' },
  { key: 'dzielnica', header: 'Dzielnica' },
  {
    key: 'wiata',
    header: 'Wiata',
    render: (item) => (item.wiata ? 'Tak' : 'Nie'),
  },
  {
    key: 'aktywny',
    header: 'Status',
    render: (item) => <StatusBadge status={item.aktywny} />,
  },
];

export function PrzystankiListPage() {
  const [search, setSearch] = useState('');
  const { page, pageSize, setPage, setPageSize } = usePagination();

  const { data, isLoading } = usePrzystanki({
    szukaj: search || undefined,
    Page: page,
    PageSize: pageSize,
  });

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-slate-900">Przystanki</h1>
          <p className="text-slate-500 mt-1">Zarządzaj przystankami w sieci komunikacyjnej</p>
        </div>
        <Button>
          <Plus className="w-4 h-4 mr-2" />
          Dodaj przystanek
        </Button>
      </div>

      <div className="flex items-center gap-4">
        <SearchInput
          value={search}
          onChange={setSearch}
          placeholder="Szukaj przystanku..."
          className="max-w-sm"
        />
      </div>

      <DataTable
        columns={columns}
        data={data?.items ?? []}
        isLoading={isLoading}
        keyExtractor={(item) => item.id}
        emptyMessage="Nie znaleziono przystanków"
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
    </div>
  );
}
