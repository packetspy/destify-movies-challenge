import { useState, useEffect } from 'react';
import '@/index.css'
import client from '@/services/api';
import Movie from '@/models/movie';

import { Table } from "@/components/ui/table"

const MovieList = () => {
  const [movies, setMovies] = useState<Movie[]>([]);

  useEffect(() => {
    client.get('/movies')
      .then((response) => setMovies(response.data))
      .catch((error) => console.error(error));
  }, []);

  const deleteMovie = (id: string) => {
    client.delete(`/movies/${id}`)
      .then(() => setMovies((prev) => prev.filter((movie) => movie.uniqueId !== id)))
      .catch((error) => console.error(error));
  };

  return(
    <>
     <Table
      data={movies}
      columns={[
        {
          title: 'First Name',
          field: 'firstName',
        },
        {
          title: 'Last Name',
          field: 'lastName',
        },
        {
          title: 'Email',
          field: 'email',
        },
        {
          title: 'Role',
          field: 'role',
        },
        {
          title: 'Created At',
          field: 'createdAt',
          Cell({ entry: { createdAt } }) {
            return <span>{formatDate(createdAt)}</span>;
          },
        },
        {
          title: '',
          field: 'id',
          Cell({ entry: { uniqueId } }) {
            return <DeleteUser id={uniqueId} />;
          },
        },
      ]}
    />
    </>
  )

  // return (

  //   <>  
  //   <Table>
  //     <TableCaption>A list of your recent invoices.</TableCaption>
  //     <TableHeader>
  //       <TableRow>
  //         <TableHead className="w-[100px]">Title</TableHead>
  //         <TableHead>Genre</TableHead>
  //         <TableHead>Rated</TableHead>
  //         <TableHead className="text-right">Year</TableHead>
  //       </TableRow>
  //     </TableHeader>
  //     <TableBody>
  //       {movies.map((movie, index) => (
  //         <TableRow key={index}>
  //           <TableCell className="font-medium">{movie.title}</TableCell>
  //           <TableCell>{movie.genre}</TableCell>
  //           <TableCell>{movie.rated}</TableCell>
  //           <TableCell className="text-right">{movie.year}</TableCell>
  //         </TableRow>
  //       ))}
  //     </TableBody>
  //   </Table>

  //     <div className="p-4">
  //       <h1 className="text-xl font-bold mb-4">Movies</h1>
  //       <ul className="space-y-2">
  //         {movies.map((movie, index) => (
  //           <li key={index} className="border p-2 flex justify-between">
  //             <span>{movie.title}</span>
  //             <Button  onClick={() => deleteMovie(movie.uniqueId)}>Delete</Button>
  //           </li>
  //         ))}
  //       </ul>
  //     </div>
  //   </>
  // );
};

export default MovieList;
