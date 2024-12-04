import { QueryClient } from '@tanstack/react-query';
import { LoaderFunctionArgs } from 'react-router-dom';

import { ContentLayout } from '@/components/layouts';
import { getMoviesQueryOptions } from '@/features/movies/api/get-movies';
import { CreateMovie } from '@/features/movies/components/create-movie';
import { MoviesList } from '@/features/movies/components/movies-list';

export const moviesLoader =
  (queryClient: QueryClient) =>
  async ({ request }: LoaderFunctionArgs) => {
    const url = new URL(request.url);

    const page = Number(url.searchParams.get('page') || 1);

    const query = getMoviesQueryOptions({ page });

    return (
      queryClient.getQueryData(query.queryKey) ??
      (await queryClient.fetchQuery(query))
    );
  };

export const MovieRoute = () => {
  return (
    <ContentLayout title="Movies">
      <div className="flex justify-end">
        <CreateMovie />
      </div>
      <div className="mt-4">
        <MoviesList/>
      </div>
    </ContentLayout>
  );
};