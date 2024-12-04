import { QueryClient } from '@tanstack/react-query';
import { ErrorBoundary } from 'react-error-boundary';
import { useParams, LoaderFunctionArgs } from 'react-router-dom';

import { ContentLayout } from '@/components/layouts';
import { Spinner } from '@/components/ui/spinner';

import {
  useMovie,
  getMovieQueryOptions,
} from '@/features/movies/api/get-movie';
import { MovieView } from '@/features/movies/components/movie-view';

export const movieLoader =
  (queryClient: QueryClient) =>
  async ({ params }: LoaderFunctionArgs) => {
    const movieUniqueId = params.movieUniqueId as string;

    const movieQuery = getMovieQueryOptions(movieUniqueId);
    
    const promises = [
      queryClient.getQueryData(movieQuery.queryKey) ??
        (await queryClient.fetchQuery(movieQuery)),
    ] as const;

    const [movie] = await Promise.all(promises);

    return {
      movie,
    };
  };

export const MovieRoute = () => {
  const params = useParams();
  const movieUniqueId = params.movieUniqueId as string;
  const movieQuery = useMovie({
    movieUniqueId,
  });

  if (movieQuery.isLoading) {
    return (
      <div className="flex h-48 w-full items-center justify-center">
        <Spinner size="lg" />
      </div>
    );
  }

  const movie = movieQuery?.data;

  if (!movie) return null;

  return (
    <>
      <ContentLayout title={movie.title}>
        <MovieView movieUniqueId={movieUniqueId} />
        <div className="mt-8">
          <ErrorBoundary
            fallback={
              <div>Failed to load comments. Try to refresh the page.</div>
            }>
          </ErrorBoundary>
        </div>
      </ContentLayout>
    </>
  );
};