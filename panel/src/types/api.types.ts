export interface PaginatedResponse<T> {
  items: T[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface PaginationParams {
  Page?: number;
  PageSize?: number;
}

export interface ApiError {
  status: number;
  title: string;
  type?: string;
  errors?: Record<string, string[]>;
}

export interface ValidationError {
  field: string;
  messages: string[];
}
