import { Spinner } from '@/components/ui/spinner';

import { useMovie } from '../api/get-movie';
import { UpdateMovie } from '../components/update-movie';
import { Link } from 'react-router-dom';
import { paths } from '@/config/paths';
import { Button } from '@/components/ui/button';
import { ArrowLeft, Link as LinkUrl } from 'lucide-react';

export const MovieView = ({ movieUniqueId }: { movieUniqueId: string }) => {
  const movieQuery = useMovie({ movieUniqueId });

  if (movieQuery.isLoading) {
    return (
      <div className="flex h-48 w-full items-center justify-center">
        <Spinner size="lg" />
      </div>
    );
  }

  const movie = movieQuery?.data?.data;

  if (!movie) return null;

  return (
    <>
      <div className="mt-6 flex flex-col ">
        <div className="flex justify-between">
          <Link to={paths.app.movies.getHref()}>
          <Button size="sm">
          <ArrowLeft className="size-4" />
            Back
          </Button></Link>
          <UpdateMovie movieUniqueId={movieUniqueId} />
        </div>
      </div>

      <div className="mt-6 flex flex-row space-y-8">
        <div className='space-x-8'>
          <img src={movie.poster} />
        </div>

        <div className='ml-5'>
          <p><b>Year:</b> {movie.year}</p>
          <p><b>Rated:</b> {movie.rated}</p>
          <p><b>Genre:</b> {movie.genre}</p>
          <p><b>Language:</b> {movie.language}</p>
          <p><b>Country:</b> {movie.country}</p>

          <p className='mt-4'><b>Actors:</b>
            {movie.actors.map((actor, index) => (
              <li key={index} className="ml-5 text-gray-700 list-none">
                <Link to={paths.app.actor.getHref(actor.uniqueId)} className='flex'>
                  <LinkUrl className="size-4 mr-2" /> {actor.name}
                </Link>
              </li>
            ))}
          </p>

          <p className='mt-4'><b>Actors:</b>
            {movie.directors.map((director, index) => (
              <li key={index} className="ml-5 text-gray-700 list-none">
                <Link to={paths.app.movies.getHref(director.uniqueId)} className='flex'>
                  <LinkUrl className="size-4 mr-2" /> {director.name}
                </Link>
              </li>
            ))}
          </p>
        
          <div className="mt-2 overflow-hidden bg-white shadow sm:rounded-lg">
            <div className="px-4 py-5 sm:px-6">
              <div className="mt-1 max-w-2xl text-sm text-gray-700">
                <p>{movie.plot} </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};
